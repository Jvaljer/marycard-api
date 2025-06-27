import 'package:cookie_jar/cookie_jar.dart';
import 'package:dio_cookie_manager/dio_cookie_manager.dart';
import 'package:marycard_client/marycard_client.dart';
import 'package:test/test.dart';
import 'package:tests/config.dart';
import 'package:tests/helper.dart';
import 'package:uuid/v4.dart';

void main() {
  group("GET", () {
    test("Get card analytics", () async {
      final api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));

      await connectAdmin(api);

      final cardService = CardService(api);
      final analyticsService = AnalyticService(api);

      final cardUid = randomUid();
      final cardResult = await cardService.createCard(cardUid);
      assert(cardResult.isRight);

      final creationResult = await analyticsService.createActivity(
          ApiActivityRequest(
              type: ApiActivityType.cardPageVisited, cardId: cardUid));
      assert(creationResult.isRight);

      final result = await analyticsService.getActivityEventsByCardId(
          cardUid, 0, DateTime.now());
      assert(result.isRight);
      assert(result.right.isNotEmpty);
      assert(result.right.first.type == ApiActivityType.cardPageVisited);
    });

    test("Get card summary", () async {
      final api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));

      await connectAdmin(api);

      final cardService = CardService(api);
      final analyticsService = AnalyticService(api);

      final cardUid = randomUid();
      final cardResult = await cardService.createCard(cardUid);
      assert(cardResult.isRight);

      final creationResult = await analyticsService.createActivity(
          ApiActivityRequest(
              type: ApiActivityType.cardPageVisited, cardId: cardUid));
      assert(creationResult.isRight);

      final result = await analyticsService.getCardActivity(cardUid);
      assert(result.isRight);
      assert(result.right.visitCount == 1);
    });

    test("Order analytics one card", () async {
      final api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));

      await connectAdmin(api);

      final orderService = OrderService(api);
      final analyticsService = AnalyticService(api);
      final cardService = CardService(api);
      final inventoryService = InventoryService(api);

      final cardUid = randomUid();

      final cardResult = await cardService.createCard(cardUid);
      assert(cardResult.isRight);

      final tagId = UuidV4().generate();
      final physicalCard = await inventoryService.createPhysicalCard(
          ApiCreatePhysicalCard(
              videoCardId: cardUid, illustrationId: null, tagId: tagId));
      assert(physicalCard.isRight);

      final physicalCardId = physicalCard.right;

      final createOrderResult = await orderService.createOrder(ApiOrderCreate(
        contactEmail: "toto@mail.com",
        customerEmail: "fafa@mail.com",
        customerFirstName: "TOTO",
        customerLastName: "FZAFA",
      ));
      assert(createOrderResult.isRight);

      final orderResult =
          await orderService.getOrderById(createOrderResult.right);
      assert(orderResult.isRight);

      final order = orderResult.right;
      final product = order.products.first;

      final createOrderItemResult =
          await orderService.createOrderItem(product.id, physicalCardId);
      assert(createOrderItemResult.isRight);

      final createEventResult =
          await analyticsService.createEvent(cardUid, "cardPageVisited");
      assert(createEventResult.isRight);

      final orderStatsResult = await analyticsService.getOrderStats(order.id);
      assert(orderStatsResult.isRight);
      final orderStats = orderStatsResult.right;
      expect(orderStats.totalCardVisited, 1);
    });

    test("Order analytics two cards", () async {
      final api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));

      await connectAdmin(api);

      final orderService = OrderService(api);
      final analyticsService = AnalyticService(api);
      final cardService = CardService(api);
      final inventoryService = InventoryService(api);

      final cardUid1 = randomUid();
      final cardUid2 = randomUid();

      final cardResult1 = await cardService.createCard(cardUid1);
      assert(cardResult1.isRight);

      final cardResult2 = await cardService.createCard(cardUid2);
      assert(cardResult2.isRight);

      final tagId1 = UuidV4().generate();
      final tagId2 = UuidV4().generate();

      final physicalCard1 = await inventoryService.createPhysicalCard(
          ApiCreatePhysicalCard(
              videoCardId: cardUid1, illustrationId: null, tagId: tagId1));
      assert(physicalCard1.isRight);

      final physicalCard2 = await inventoryService.createPhysicalCard(
          ApiCreatePhysicalCard(
              videoCardId: cardUid2, illustrationId: null, tagId: tagId2));
      assert(physicalCard2.isRight);

      final physicalCardId1 = physicalCard1.right;
      final physicalCardId2 = physicalCard2.right;

      final createOrderResult = await orderService.createOrder(ApiOrderCreate(
        contactEmail: "toto@mail.com",
        customerEmail: "fafa@mail.com",
        customerFirstName: "TOTO",
        customerLastName: "FZAFA",
      ));
      assert(createOrderResult.isRight);

      final orderResult =
          await orderService.getOrderById(createOrderResult.right);
      assert(orderResult.isRight);

      final order = orderResult.right;
      final product = order.products.first;

      final createOrderItemResult1 =
          await orderService.createOrderItem(product.id, physicalCardId1);
      expect(createOrderItemResult1.isRight, isTrue);

      final createOrderItemResult2 =
          await orderService.createOrderItem(product.id, physicalCardId2);
      expect(createOrderItemResult2.isRight, isTrue);

      final createEventResult1 =
          await analyticsService.createEvent(cardUid1, "cardPageVisited");
      assert(createEventResult1.isRight);
      final createEventResult2 =
          await analyticsService.createEvent(cardUid2, "cardPageVisited");
      assert(createEventResult2.isRight);

      final orderStatsResult = await analyticsService.getOrderStats(order.id);
      assert(orderStatsResult.isRight);
      final orderStats = orderStatsResult.right;
      expect(orderStats.totalCardVisited, 2);

      final res = await orderService.deleteOrderItem(order.id, physicalCardId1);
      assert(res.isRight);

      final orderStats2 = await analyticsService.getOrderStats(order.id);
      assert(orderStats2.isRight);
      final orderStats2Result = orderStats2.right;
      expect(orderStats2Result.totalCardVisited, 1);
    });

    test("Order analytics one card three taps", () async {
      final api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));

      await connectAdmin(api);

      final orderService = OrderService(api);
      final analyticsService = AnalyticService(api);
      final cardService = CardService(api);
      final inventoryService = InventoryService(api);

      final cardUid = randomUid();

      final cardResult = await cardService.createCard(cardUid);
      assert(cardResult.isRight);

      final tagId = UuidV4().generate();
      final physicalCard = await inventoryService.createPhysicalCard(
          ApiCreatePhysicalCard(
              videoCardId: cardUid, illustrationId: null, tagId: tagId));
      assert(physicalCard.isRight);

      final physicalCardId = physicalCard.right;

      final createOrderResult = await orderService.createOrder(ApiOrderCreate(
        contactEmail: "toto@mail.com",
        customerEmail: "fafa@mail.com",
        customerFirstName: "TOTO",
        customerLastName: "FZAFA",
      ));
      assert(createOrderResult.isRight);

      final orderResult =
          await orderService.getOrderById(createOrderResult.right);
      assert(orderResult.isRight);

      final order = orderResult.right;
      final product = order.products.first;

      final createOrderItemResult =
          await orderService.createOrderItem(product.id, physicalCardId);
      assert(createOrderItemResult.isRight);

      await analyticsService.createEvent(cardUid, "cardPageVisited");
      await analyticsService.createEvent(cardUid, "cardPageVisited");
      await analyticsService.createEvent(cardUid, "cardPageVisited");

      final orderStatsResult = await analyticsService.getOrderStats(order.id);
      assert(orderStatsResult.isRight);
      final orderStats = orderStatsResult.right;
      expect(orderStats.totalCardVisited, 3);
    });

    test("Order analytics two cards 5 taps", () async {
      final api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));

      await connectAdmin(api);

      final orderService = OrderService(api);
      final analyticsService = AnalyticService(api);
      final cardService = CardService(api);
      final inventoryService = InventoryService(api);

      final cardUid1 = randomUid();
      final cardUid2 = randomUid();

      final cardResult1 = await cardService.createCard(cardUid1);
      assert(cardResult1.isRight);

      final cardResult2 = await cardService.createCard(cardUid2);
      assert(cardResult2.isRight);

      final tagId1 = UuidV4().generate();
      final tagId2 = UuidV4().generate();

      final physicalCard1 = await inventoryService.createPhysicalCard(
          ApiCreatePhysicalCard(
              videoCardId: cardUid1, illustrationId: null, tagId: tagId1));
      assert(physicalCard1.isRight);

      final physicalCard2 = await inventoryService.createPhysicalCard(
          ApiCreatePhysicalCard(
              videoCardId: cardUid2, illustrationId: null, tagId: tagId2));
      assert(physicalCard2.isRight);

      final physicalCardId1 = physicalCard1.right;
      final physicalCardId2 = physicalCard2.right;

      final createOrderResult = await orderService.createOrder(ApiOrderCreate(
        contactEmail: "toto@mail.com",
        customerEmail: "fafa@mail.com",
        customerFirstName: "TOTO",
        customerLastName: "FZAFA",
      ));
      assert(createOrderResult.isRight);

      final orderResult =
          await orderService.getOrderById(createOrderResult.right);
      assert(orderResult.isRight);

      final order = orderResult.right;
      final product = order.products.first;

      final createOrderItemResult1 =
          await orderService.createOrderItem(product.id, physicalCardId1);
      expect(createOrderItemResult1.isRight, isTrue);

      final createOrderItemResult2 =
          await orderService.createOrderItem(product.id, physicalCardId2);
      expect(createOrderItemResult2.isRight, isTrue);

      await analyticsService.createEvent(cardUid1, "cardPageVisited");
      await analyticsService.createEvent(cardUid1, "cardPageVisited");
      await analyticsService.createEvent(cardUid1, "cardPageVisited");

      await analyticsService.createEvent(cardUid2, "cardPageVisited");
      await analyticsService.createEvent(cardUid2, "cardPageVisited");

      final orderStatsResult = await analyticsService.getOrderStats(order.id);
      assert(orderStatsResult.isRight);
      final orderStats = orderStatsResult.right;
      expect(orderStats.totalCardVisited, 5);

      final res = await orderService.deleteOrderItem(order.id, physicalCardId1);
      assert(res.isRight);

      final orderStats2 = await analyticsService.getOrderStats(order.id);
      assert(orderStats2.isRight);
      final orderStats2Result = orderStats2.right;
      expect(orderStats2Result.totalCardVisited, 2);
    });
  });
}
