import 'package:cookie_jar/cookie_jar.dart';
import 'package:dio_cookie_manager/dio_cookie_manager.dart';
import 'package:marycard_client/marycard_client.dart';
import 'package:test/test.dart';
import 'package:tests/config.dart';
import 'package:tests/helper.dart';
import 'package:tests/uploader.dart';

void main() {
  group("Post", () {
    test('Create a new group without a preview video', () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final groupService = GroupService(api);

      final groupResult =
          await groupService.createGroup(ApiCreateGroup(name: "TOTO CARDS"));

      expect(groupResult.isRight, isTrue);
    });

    test('Create a new group with a preview video', () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final groupService = GroupService(api);
      final videoService = VideoService(api);

      final apiVideoId = await uploadSample();

      final videoResult = await videoService.createPreviewVideo(
          ApiVideoCreate(apiVideoId: apiVideoId, title: "WOWOWOWO"));
      expect(videoResult.isRight, isTrue);

      final groupResult = await groupService.createGroup(ApiCreateGroup(
          name: "TOTO CARDS", previewVideoId: videoResult.right));

      expect(groupResult.isRight, isTrue);

      final getGroupResult = await groupService.getGroup(groupResult.right);
      expect(getGroupResult.isRight, isTrue);
      expect(getGroupResult.right.previewVideo, isNotNull);
      expect(getGroupResult.right.previewVideo!.id, videoResult.right);
    });
  });

  group("GET", () {
    test('Get groups filter by name', () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final groupService = GroupService(api);

      final randomName = randomUid();

      final groupResult =
          await groupService.createGroup(ApiCreateGroup(name: randomName));
      expect(groupResult.isRight, isTrue);

      final groupsResult = await groupService.getGroupList(
          page: 0, until: DateTime.now(), name: randomName);

      expect(groupsResult.isRight, isTrue);
      expect(groupsResult.right, isNotEmpty);
      expect(groupsResult.right.first.name, randomName);
    });

    test("Get group by ID", () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final groupService = GroupService(api);

      final randomName = randomUid();

      final groupResult =
          await groupService.createGroup(ApiCreateGroup(name: randomName));
      expect(groupResult.isRight, isTrue);

      final group = await groupService.getGroup(groupResult.right);

      expect(group.isRight, isTrue);
      expect(group.right.name, randomName);
    });

    test("Assign card to group", () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final groupService = GroupService(api);
      final cardService = CardService(api);

      final randomName = randomUid();

      final groupResult =
          await groupService.createGroup(ApiCreateGroup(name: randomName));
      expect(groupResult.isRight, isTrue);

      final cardUId = randomUid();
      final cardResult = await cardService.createCard(cardUId);
      expect(cardResult.isRight, isTrue);

      final assignResult =
          await cardService.updateCardGroup(cardUId, groupResult.right);
      expect(assignResult.isRight, isTrue);

      final getCardResult = await cardService.getCardById(cardUId);
      expect(getCardResult.isRight, isTrue);
      expect(getCardResult.right.group, isNotNull);
      expect(getCardResult.right.group!.id, groupResult.right);
    });

    test("Get card list by group id", () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final groupService = GroupService(api);
      final cardService = CardService(api);

      final randomName = randomUid();

      final groupResult =
          await groupService.createGroup(ApiCreateGroup(name: randomName));
      expect(groupResult.isRight, isTrue);

      final cardUId = randomUid();
      final cardResult = await cardService.createCard(cardUId);
      expect(cardResult.isRight, isTrue);

      final assignResult =
          await cardService.updateCardGroup(cardUId, groupResult.right);
      expect(assignResult.isRight, isTrue);

      final cardsResult = await cardService.getCardListByGroupId(
          groupResult.right,
          page: 0,
          until: DateTime.now());
      expect(cardsResult.isRight, isTrue);
      expect(cardsResult.right, isNotEmpty);
      expect(cardsResult.right.first.id, cardUId);
    });

    test("Update group preview video and name", () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final groupService = GroupService(api);
      final videoService = VideoService(api);

      final apiVideoId = await uploadSample();

      final videoResult = await videoService.createPreviewVideo(
          ApiVideoCreate(apiVideoId: apiVideoId, title: "BIBIBIBI"));
      expect(videoResult.isRight, isTrue);

      final randomName = randomUid();

      final groupResult =
          await groupService.createGroup(ApiCreateGroup(name: randomName));
      expect(groupResult.isRight, isTrue);

      final updateResult = await groupService.updateGroup(groupResult.right,
          ApiUpdateGroup(name: "NEW NAME", previewVideoId: videoResult.right));
      expect(updateResult.isRight, isTrue);

      final getGroupResult = await groupService.getGroup(groupResult.right);
      expect(getGroupResult.isRight, isTrue);
      expect(getGroupResult.right.name, "NEW NAME");
      expect(getGroupResult.right.previewVideo, isNotNull);
      expect(getGroupResult.right.previewVideo!.id, videoResult.right);
    });
  });
}
