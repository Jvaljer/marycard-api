import 'package:cookie_jar/cookie_jar.dart';
import 'package:dio_cookie_manager/dio_cookie_manager.dart';
import 'package:marycard_client/marycard_client.dart';
import 'package:test/test.dart';
import 'package:tests/config.dart';
import 'package:tests/helper.dart';
import 'package:uuid/uuid.dart';

void main() {
  group("Physical card endpoints", () {
    test("PUT", () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final inventoryService = InventoryService(api);
      final cardService = CardService(api);

      final videoCardId = randomUid();

      final createCard = await cardService.createCard(videoCardId);
      expect(createCard.isRight, isTrue);

      final createIllustration = await inventoryService.createIllustration(
          ApiCreateIllustration(name: "DSD", width: 100, height: 100));
      expect(createIllustration.isRight, isTrue);

      final createPhysicalCard = await inventoryService.createPhysicalCard(
          ApiCreatePhysicalCard(
              videoCardId: videoCardId,
              illustrationId: createIllustration.right,
              tagId: Uuid().v4().toString()));
      expect(createPhysicalCard.isRight, isTrue);

      final updatePhysicalCard = await inventoryService.updatePhysicalCard(
          createPhysicalCard.right,
          ApiUpdatePhysicalCard(
            note: "Note",
            warehouseCountryCode: "US",
            illustrationId: null,
          ));

      expect(updatePhysicalCard.isRight, isTrue);

      final getPhysicalCard =
          await inventoryService.getPhysicalCard(createPhysicalCard.right);

      expect(getPhysicalCard.isRight, isTrue);
      expect(getPhysicalCard.right.note, "Note");
      expect(getPhysicalCard.right.warehouseCountryCode, "US");

      final updatePhysicalCard2 = await inventoryService.updatePhysicalCard(
          createPhysicalCard.right,
          ApiUpdatePhysicalCard(
            note: null,
            warehouseCountryCode: null,
            illustrationId: null,
          ));

      expect(updatePhysicalCard2.isRight, isTrue);

      final getPhysicalCard2 =
          await inventoryService.getPhysicalCard(createPhysicalCard.right);

      expect(getPhysicalCard2.isRight, isTrue);
      expect(getPhysicalCard2.right.note, null);
      expect(getPhysicalCard2.right.warehouseCountryCode, null);
    });

    test("Update illustration ID", () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final inventoryService = InventoryService(api);

      final createPhysicalCard = await inventoryService.createPhysicalCard(
          ApiCreatePhysicalCard(
              videoCardId: randomUid(),
              illustrationId: null,
              tagId: Uuid().v4().toString()));

      expect(createPhysicalCard.isRight, isTrue);

      final firstGet = await inventoryService
          .getPhysicalCard(createPhysicalCard.right); // Illustration is null
      expect(firstGet.isRight, isTrue);
      expect(firstGet.right.illustration, isNull);

      final illustrationResult = await inventoryService.createIllustration(
          ApiCreateIllustration(name: "DSD", width: 100, height: 100));
      expect(illustrationResult.isRight, isTrue);

      final updatePhysicalCard = await inventoryService.updatePhysicalCard(
          createPhysicalCard.right,
          ApiUpdatePhysicalCard(
              illustrationId: illustrationResult.right,
              note: null,
              warehouseCountryCode: null));

      expect(updatePhysicalCard.isRight, isTrue);

      final getPhysicalCard =
          await inventoryService.getPhysicalCard(createPhysicalCard.right);
      expect(getPhysicalCard.isRight, isTrue);
      expect(getPhysicalCard.right.illustration, isNotNull);
      expect(getPhysicalCard.right.illustration!.id, illustrationResult.right);
    });

    test("Get physical card tag data", () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final inventoryService = InventoryService(api);
      final cardService = CardService(api);
      final videoCardId = randomUid();

      final createCard = await cardService.createCard(videoCardId);
      expect(createCard.isRight, isTrue);

      final tagId = Uuid().v4().toString();
      final createPhysicalCard = await inventoryService.createPhysicalCard(
          ApiCreatePhysicalCard(
              videoCardId: videoCardId, illustrationId: null, tagId: tagId));
      expect(createPhysicalCard.isRight, isTrue);

      final getPhysicalCard =
          await inventoryService.getPhysicalCard(createPhysicalCard.right);
      expect(getPhysicalCard.isRight, isTrue);
      expect(getPhysicalCard.right.tagId, tagId);
      expect(getPhysicalCard.right.physicalTag, isNotNull);
      expect(getPhysicalCard.right.physicalTag!.id, tagId);
      expect(getPhysicalCard.right.physicalTag!.physicalUid, tagId);
    });
  });
}
