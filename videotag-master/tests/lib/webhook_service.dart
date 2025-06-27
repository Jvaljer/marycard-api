import 'dart:convert';
import 'dart:io';

import 'package:marycard_client/marycard_client.dart';

class WebhookService {
  final Api _api;

  const WebhookService(this._api);

  Future<String> getTemplate(String templatePath) async {
    return await File(templatePath).readAsString();
  }

  Future<int> send(String path, Map<String, dynamic> payload) async {
    var res = await _api.dio.post("/v1/sales/webhooks/$path", data: payload);
    return res.statusCode ?? 500;
  }

  Future<int> createProduct(
      {required int id,
      required int variantId,
      required String name,
      required String description,
      required String variantName,
      required String sku}) async {
    const String templatePath = "templates/products_create.json";

    final String json = (await getTemplate(templatePath))
        .replaceAll("{{id}}", id.toString())
        .replaceAll("{{variantId}}", variantId.toString())
        .replaceAll("{{name}}", name)
        .replaceAll("{{description}}", description)
        .replaceAll("{{variantName}}", variantName)
        .replaceAll("{{sku}}", sku);

    return await send("products/created", jsonDecode(json));
  }

  Future<int> updateProduct(
      {required int id,
      required int variantId,
      required String name,
      required String description,
      required String variantName,
      required String sku}) async {
    const String templatePath = "templates/products_update.json";

    final String json = (await getTemplate(templatePath))
        .replaceAll("{{id}}", id.toString())
        .replaceAll("{{variantId}}", variantId.toString())
        .replaceAll("{{name}}", name)
        .replaceAll("{{description}}", description)
        .replaceAll("{{variantName}}", variantName)
        .replaceAll("{{sku}}", sku);

    return await send("products/updated", jsonDecode(json));
  }

  Future<int> deleteProduct({required int id}) async {
    const String templatePath = "templates/products_delete.json";

    final String json =
        (await getTemplate(templatePath)).replaceAll("{{id}}", id.toString());
    return await send("products/deleted", jsonDecode(json));
  }

  Future<int> createOrder(
      {required int id,
      required int productId,
      required int variantId,
      required int quantity,
      required int customerId,
      DateTime? createdAt,
      DateTime? updatedAt}) async {
    const String templatePath = "templates/order_create.json";

    final String json = (await getTemplate(templatePath))
        .replaceAll("{{id}}", id.toString())
        .replaceAll("{{product_id}}", productId.toString())
        .replaceAll("{{variant_id}}", variantId.toString())
        .replaceAll("{{customer_id}}", customerId.toString())
        .replaceAll("{{product_quantity}}", quantity.toString())
        .replaceAll("{{created_at}}",
            createdAt?.toIso8601String() ?? DateTime.now().toIso8601String())
        .replaceAll("{{updated_at}}",
            updatedAt?.toIso8601String() ?? DateTime.now().toIso8601String());

    return await send("orders/created", jsonDecode(json));
  }

  Future<int> updateOrder(
      {required int id,
      required int productId,
      required int variantId,
      required int quantity,
      required int customerId,
      DateTime? createdAt,
      DateTime? updatedAt}) async {
    const String templatePath = "templates/order_create.json";

    final String json = (await getTemplate(templatePath))
        .replaceAll("{{id}}", id.toString())
        .replaceAll("{{product_id}}", productId.toString())
        .replaceAll("{{variant_id}}", variantId.toString())
        .replaceAll("{{customer_id}}", customerId.toString())
        .replaceAll("{{product_quantity}}", quantity.toString())
        .replaceAll("{{created_at}}",
            createdAt?.toIso8601String() ?? DateTime.now().toIso8601String())
        .replaceAll("{{updated_at}}",
            updatedAt?.toIso8601String() ?? DateTime.now().toIso8601String());

    return await send("orders/updated", jsonDecode(json));
  }
}
