import 'package:cookie_jar/cookie_jar.dart';
import 'package:dio_cookie_manager/dio_cookie_manager.dart';
import 'package:tests/config.dart';
import 'package:test/test.dart';
import 'package:marycard_client/marycard_client.dart';
import 'package:tests/helper.dart';
import 'package:tests/webhook_service.dart';
import 'package:uuid/v4.dart';

void main() {
  group("Products", () {
    test('Products create', () async {
      var api = await Api.create(Config(apiUrl: url));
      var adminApi = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      int randomId = DateTime.now().millisecondsSinceEpoch;
      await connectAdmin(adminApi);

      var webhookService = WebhookService(api);
      var inventoryService = InventoryService(adminApi);

      int variantRandomId = DateTime.now().millisecondsSinceEpoch;

      var res = await webhookService.createProduct(
          id: randomId,
          variantId: variantRandomId,
          name: "Product $randomId",
          description: "<p>Description $randomId</p>",
          variantName: "Variant $variantRandomId",
          sku: "SKU $randomId");
      expect(res, 200);

      var product = await inventoryService.getByShopifyVariantId(
          randomId.toString(), variantRandomId.toString());
      expect(product.isRight, isTrue);
      expect(product.right.name, "Product $randomId");
      expect(product.right.variantName, "Variant $variantRandomId");
      expect(product.right.sku, "SKU $randomId");
      expect(product.right.deleted, isFalse);
      expect(product.right.shopifyProductId.productId, randomId);
      expect(product.right.shopifyProductId.variantId, variantRandomId);

      var productGet = await inventoryService.getByShopifyVariantId(
          product.right.shopifyProductId.productId.toString(),
          product.right.shopifyProductId.variantId.toString());
      expect(productGet.isRight, isTrue);
    });

    test('Products update', () async {
      var api = await Api.create(Config(apiUrl: url));
      var adminApi = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      int randomId = DateTime.now().millisecondsSinceEpoch;
      await connectAdmin(adminApi);

      var webhookService = WebhookService(api);
      var inventoryService = InventoryService(adminApi);

      int variantRandomId = DateTime.now().millisecondsSinceEpoch;

      var res = await webhookService.createProduct(
          id: randomId,
          variantId: variantRandomId,
          name: "Product $randomId",
          description: "Description $randomId",
          variantName: "Variant $variantRandomId",
          sku: "SKU $randomId");
      expect(res, 200);

      var resUpdate = await webhookService.updateProduct(
          id: randomId,
          variantId: variantRandomId,
          name: "Product $randomId Updated",
          description: "Description $randomId Updated",
          variantName: "Variant $variantRandomId Updated",
          sku: "SKU $randomId Updated");
      expect(resUpdate, 200);

      var product = await inventoryService.getByShopifyVariantId(
          randomId.toString(), variantRandomId.toString());
      expect(product.isRight, isTrue);
      expect(product.right.name, "Product $randomId Updated");
      expect(product.right.variantName, "Variant $variantRandomId Updated");
      expect(product.right.sku, "SKU $randomId Updated");
      expect(product.right.deleted, isFalse);
      expect(product.right.shopifyProductId.productId, randomId);
      expect(product.right.shopifyProductId.variantId, variantRandomId);
    });

    test('Products delete', () async {
      var api = await Api.create(Config(apiUrl: url));
      var adminApi = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      int randomId = DateTime.now().millisecondsSinceEpoch;
      await connectAdmin(adminApi);

      var webhookService = WebhookService(api);
      var inventoryService = InventoryService(adminApi);

      int variantRandomId = DateTime.now().millisecondsSinceEpoch;

      var res = await webhookService.createProduct(
          id: randomId,
          variantId: variantRandomId,
          name: "Product $randomId",
          description: "Description $randomId",
          variantName: "Variant $variantRandomId",
          sku: "SKU $randomId");
      expect(res, 200);

      var resDelete = await webhookService.deleteProduct(id: randomId);
      expect(resDelete, 200);

      var product = await inventoryService.getByShopifyVariantId(
          randomId.toString(), variantRandomId.toString());
      expect(product.isRight, isTrue);
      expect(product.right.deleted, isTrue);
    });
  });

  group("Orders", () {
    test('Order created', () async {
      final api = await Api.create(Config(apiUrl: url));
      var adminApi = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(adminApi);

      final webhookService = WebhookService(api);
      int randomId = DateTime.now().millisecondsSinceEpoch;
      int productId = DateTime.now().millisecondsSinceEpoch + 1;
      int variantId = DateTime.now().millisecondsSinceEpoch + 2;
      int customerId = DateTime.now().millisecondsSinceEpoch + 3;

      final res = await webhookService.createOrder(
          id: randomId,
          quantity: 2,
          productId: productId,
          variantId: variantId,
          customerId: customerId);
      expect(res, 201);
    });

    test('0rder updated', () async {
      final api = await Api.create(Config(apiUrl: url));
      var adminApi = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(adminApi);

      final orderService = OrderService(adminApi);

      final webhookService = WebhookService(api);
      int randomId = DateTime.now().millisecondsSinceEpoch;
      int productId = DateTime.now().millisecondsSinceEpoch + 1;
      int variantId = DateTime.now().millisecondsSinceEpoch + 2;
      int customerId = DateTime.now().millisecondsSinceEpoch + 3;

      final res = await webhookService.createOrder(
          id: randomId,
          quantity: 2,
          productId: productId,
          variantId: variantId,
          customerId: customerId);
      expect(res, 201);

      int newProductId = DateTime.now().millisecondsSinceEpoch + 4;
      int newVariantId = DateTime.now().millisecondsSinceEpoch + 5;

      final resUpdate = await webhookService.updateOrder(
          id: randomId,
          quantity: 3,
          productId: newProductId,
          variantId: newVariantId,
          customerId: customerId);

      expect(resUpdate, 200);

      final orderResult = await orderService.getOrderByShopifyId(randomId);
      expect(orderResult.isRight, isTrue);
      final order = orderResult.right;

      // Updating the order should delete the products that are not in the updated order
      expect(order.products.length, 2);
    });

    test("Create order item", () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final orderService = OrderService(api);
      final inventoryService = InventoryService(api);
      final cardService = CardService(api);

      final webhookService = WebhookService(api);
      int randomId = DateTime.now().millisecondsSinceEpoch;
      int productId = DateTime.now().millisecondsSinceEpoch + 1;
      int variantId = DateTime.now().millisecondsSinceEpoch + 2;
      int customerId = DateTime.now().millisecondsSinceEpoch + 3;

      final res = await webhookService.createOrder(
          id: randomId,
          quantity: 2,
          productId: productId,
          variantId: variantId,
          customerId: customerId);
      expect(res, 201);

      final orderResult = await orderService.getOrderByShopifyId(randomId);
      expect(orderResult.isRight, isTrue);

      final videoCardId = randomUid();
      final cardResult = await cardService.createCard(videoCardId);
      expect(cardResult.isRight, isTrue);

      final illustrationResult = await inventoryService.createIllustration(
          ApiCreateIllustration(
              name: "My illustration", width: 400, height: 400));
      expect(illustrationResult.isRight, isTrue);

      final physicalCardResult = await inventoryService.createPhysicalCard(
          ApiCreatePhysicalCard(
              videoCardId: videoCardId,
              illustrationId: illustrationResult.right,
              tagId: UuidV4().generate()));
      expect(physicalCardResult.isRight, isTrue);

      final orderProductId = orderResult.right.products.first.id;

      final orderItemResult = await orderService.createOrderItem(
          orderProductId, physicalCardResult.right);
      expect(orderItemResult.isRight, isTrue);

      final orderResult2 = await orderService.getOrderByShopifyId(randomId);
      expect(orderResult2.isRight, isTrue);
      final items = orderResult2.right.products
          .where((p) => p.id == orderProductId)
          .first
          .items;
      expect(items.length, 1);
      expect(items.first.physicalCardId, physicalCardResult.right);

      final deleteResult = await orderService.deleteOrderItem(
          orderResult2.right.id, physicalCardResult.right);
      expect(deleteResult.isRight, isTrue);

      final orderResult3 = await orderService.getOrderByShopifyId(randomId);
      expect(orderResult3.isRight, isTrue);

      final items2 = orderResult3.right.products
          .where((p) => p.id == orderProductId)
          .first
          .items;
      expect(items2.length, 0);
    });
  });
}
