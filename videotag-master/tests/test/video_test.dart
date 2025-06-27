import 'package:cookie_jar/cookie_jar.dart';
import 'package:tests/config.dart';
import 'package:test/test.dart';
import 'package:marycard_client/marycard_client.dart';
import 'package:dio_cookie_manager/dio_cookie_manager.dart';
import 'package:tests/helper.dart';
import 'package:tests/uploader.dart';

void main() {
  group("Admin endpoints should be protected", () {
    test('List videos', () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      var cardService = CardService(api);

      final result = await cardService.getAllCards(0, DateTime.now());
      expect(result.isLeft, isTrue);
    });

    test('Search video', () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      var cardService = CardService(api);

      final result = await cardService.searchCards("aaa", 0, DateTime.now());
      expect(result.isLeft, isTrue);
    });
  });

  group("Video upload", () {
    test('Create video', () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));

      await connectAdmin(api);

      var cardService = CardService(api);

      final cardUid = randomUid();

      final creationResult = await cardService.createCard(cardUid);
      expect(creationResult.isRight, isTrue);

      var videoId = await uploadSample();

      var create = await cardService.updateVideoOnCard(
          cardUid, ApiVideoCreate(apiVideoId: videoId, title: "ARCANE S3 EP1"));
      expect(create.isRight, isTrue);

      final card = await cardService.getCardById(cardUid);

      expect(card.isRight, isTrue);
      expect(card.right.locked, isFalse);
      expect(card.right.video, isNotNull);
      expect(card.right.video!.title, "ARCANE S3 EP1");
      expect(card.right.video!.videoUrl.endsWith(videoId), isTrue);
    });
  });

  group("Video Locking", () {
    test('Lock video', () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));

      await connectAdmin(api);

      var cardService = CardService(api);

      final cardUid = randomUid();

      final creationResult = await cardService.createCard(cardUid);
      expect(creationResult.isRight, isTrue);
      var videoId = await uploadSample();

      var create = await cardService.updateVideoOnCard(
          cardUid, ApiVideoCreate(apiVideoId: videoId, title: "ARCANE S3 EP1"));

      expect(create.isRight, isTrue);

      var lock = await cardService.lockCard(cardUid);
      expect(lock.isRight, isTrue);

      var videoGet = await cardService.getCardById(cardUid);
      expect(videoGet.isRight, isTrue);
      expect(videoGet.right.locked, isTrue);
    });

    test('Unlock video', () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));

      var notConnectedApi = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      var cardService = CardService(api);

      final cardUid = randomUid();

      final creationResult = await cardService.createCard(cardUid);
      expect(creationResult.isRight, isTrue);

      var videoId = await uploadSample();

      var create = await cardService.updateVideoOnCard(
          cardUid, ApiVideoCreate(apiVideoId: videoId, title: "ARCANE S3 EP1"));
      expect(create.isRight, isTrue);

      var lock = await cardService.lockCard(cardUid);
      expect(lock.isRight, isTrue);

      final unlockResult =
          await CardService(notConnectedApi).unlockCard(cardUid);

      expect(unlockResult.isLeft, isTrue);

      var unlock = await cardService.unlockCard(cardUid);
      expect(unlock.isRight, isTrue);

      var videoGet = await cardService.getCardById(cardUid);
      expect(videoGet.isRight, isTrue);
      expect(videoGet.right.locked, isFalse);
    });
  });

  group("Search video", () {
    test('Search video', () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));

      await connectAdmin(api);

      var cardService = CardService(api);

      final cardUid = randomUid();

      final creationResult = await cardService.createCard(cardUid);
      expect(creationResult.isRight, isTrue);

      var videoId = await uploadSample();

      var create = await cardService.updateVideoOnCard(
          cardUid, ApiVideoCreate(apiVideoId: videoId, title: "ARCANE S3 EP1"));
      expect(create.isRight, isTrue);

      var search = await cardService.searchCards(cardUid, 0, DateTime.now());
      expect(search.isRight, isTrue);
      expect(search.right.length, 1);
      expect(search.right.first.id, cardUid);
    });
  });

  group("Preview video", () {
    test('Admin create preview video', () async {
      final api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final videoService = VideoService(api);

      final videoId = await uploadSample();

      final create = await videoService.createPreviewVideo(
          ApiVideoCreate(apiVideoId: videoId, title: "Happy birthday"));

      expect(create.isRight, isTrue);
    });

    test('Admin assign preview video on card', () async {
      final api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final videoService = VideoService(api);

      final apiVideoId = await uploadSample();

      final videoId = await videoService.createPreviewVideo(
          ApiVideoCreate(apiVideoId: apiVideoId, title: "Happy birthday"));

      expect(videoId.isRight, isTrue);

      final cardService = CardService(api);

      final cardUid = randomUid();

      final creationResult = await cardService.createCard(cardUid);
      expect(creationResult.isRight, isTrue);

      final assign =
          await cardService.updatePreviewVideoOnCard(cardUid, videoId.right);
      expect(assign.isRight, isTrue);

      final card = await cardService.getCardById(cardUid);
      expect(card.isRight, isTrue);
      expect(card.right.previewVideo, isNotNull);
      expect(card.right.previewVideo!.title, "Happy birthday");

      final assignNull =
          await cardService.updatePreviewVideoOnCard(cardUid, null);
      expect(assignNull.isRight, isTrue);
      final cardNull = await cardService.getCardById(cardUid);
      expect(cardNull.isRight, isTrue);
      expect(cardNull.right.previewVideo, isNull);
    });

    test('Preview video getters', () async {
      final api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final videoService = VideoService(api);

      final apiVideoId = await uploadSample();

      final videoId = await videoService.createPreviewVideo(
          ApiVideoCreate(apiVideoId: apiVideoId, title: "Happy birthday"));

      expect(videoId.isRight, isTrue);

      final preview = await videoService.getPreviewVideo(videoId.right);
      expect(preview.isRight, isTrue);
      expect(preview.right.id, videoId.right);
      expect(preview.right.title, "Happy birthday");

      final previewVideos =
          await videoService.getPreviewVideos(0, DateTime.now());
      expect(previewVideos.isRight, isTrue);
      expect(previewVideos.right.isNotEmpty, isTrue);
      expect(previewVideos.right.any((v) => v.id == videoId.right), isTrue);

      final previewSearchByTitle =
          await videoService.searchPreviewVideos("Happy", 0, DateTime.now());
      expect(previewSearchByTitle.isRight, isTrue);
      expect(previewSearchByTitle.right.isNotEmpty, isTrue);
      expect(
          previewSearchByTitle.right.any((v) => v.id == videoId.right), isTrue);

      final previewSearchByVideoId =
          await videoService.searchPreviewVideos(apiVideoId, 0, DateTime.now());
      expect(previewSearchByVideoId.isRight, isTrue);
      expect(previewSearchByVideoId.right.isNotEmpty, isTrue);
      expect(previewSearchByVideoId.right.any((v) => v.id == videoId.right),
          isTrue);
    });

    test("Update video card url", () async {
      final api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final cardService = CardService(api);
      final cardUid = randomUid();
      final creationResult = await cardService.createCard(cardUid);
      expect(creationResult.isRight, isTrue);

      final updateCardUrl =
          await cardService.updateCardUrl(cardUid, url: "https://example.com");
      expect(updateCardUrl.isRight, isTrue);
      final card = await cardService.getCardById(cardUid);
      expect(card.isRight, isTrue);
      expect(card.right.url, isNotNull);
      expect(card.right.url, "https://example.com");
    });
  });
}
