import 'package:cookie_jar/cookie_jar.dart';
import 'package:dio_cookie_manager/dio_cookie_manager.dart';
import 'package:marycard_client/marycard_client.dart';
import 'package:test/test.dart';
import 'package:tests/config.dart';
import 'package:tests/helper.dart';

void main() {
  group("Order test", () {
    test("Create fake order", () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final orderService = OrderService(api);

      final createOrderResult = await orderService.createOrder(ApiOrderCreate(
          contactEmail: "toto@mail.com", customerEmail: "fafa@mail.com"));

      expect(createOrderResult.isRight, isTrue);

      final orderId = createOrderResult.right;

      final getOrderResult = await orderService.getOrderById(orderId);

      expect(getOrderResult.isRight, isTrue);
      expect(getOrderResult.right.contactEmail, "toto@mail.com");
      expect(getOrderResult.right.customerEmail, "fafa@mail.com");
    });

    test("Create fake order with customer phone and note", () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final orderService = OrderService(api);

      final createOrderResult = await orderService.createOrder(
        ApiOrderCreate(
            contactEmail: "toto@mail.com",
            customerEmail: "fafa@mail.com",
            customerPhone: "+33606060606",
            note: "ORDER FROM GOGO"),
      );

      expect(createOrderResult.isRight, isTrue);

      final orderId = createOrderResult.right;

      final getOrderResult = await orderService.getOrderById(orderId);

      expect(getOrderResult.isRight, isTrue);
      expect(getOrderResult.right.contactEmail, "toto@mail.com");
      expect(getOrderResult.right.customerEmail, "fafa@mail.com");
      expect(getOrderResult.right.customerPhone, "+33606060606");
      expect(getOrderResult.right.note, "ORDER FROM GOGO");
    });

    test("Create fake order with customer names", () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final orderService = OrderService(api);

      final createOrderResult = await orderService.createOrder(
        ApiOrderCreate(
            contactEmail: "toto@mail.com",
            customerEmail: "fafa@mail.com",
            customerPhone: "+33606060606",
            customerFirstName: "Toto",
            customerLastName: "Fafa",
            note: "ORDER FROM GOGO"),
      );

      expect(createOrderResult.isRight, isTrue);

      final orderId = createOrderResult.right;

      final getOrderResult = await orderService.getOrderById(orderId);

      expect(getOrderResult.isRight, isTrue);
      expect(getOrderResult.right.contactEmail, "toto@mail.com");
      expect(getOrderResult.right.customerEmail, "fafa@mail.com");
      expect(getOrderResult.right.customerPhone, "+33606060606");
      expect(getOrderResult.right.note, "ORDER FROM GOGO");
      expect(getOrderResult.right.customerFirstName, "Toto");
      expect(getOrderResult.right.customerLastName, "Fafa");
    });

    test("Create and update order", () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final orderService = OrderService(api);

      final createOrderResult = await orderService.createOrder(
        ApiOrderCreate(
            contactEmail: "toto@mail.com",
            customerEmail: "fafa@mail.com",
            customerPhone: "+33606060606",
            customerFirstName: "Toto",
            customerLastName: "Fafa",
            note: "ORDER FROM GOGO"),
      );
      expect(createOrderResult.isRight, isTrue);

      final orderId = createOrderResult.right;

      final updateOrderResult = await orderService.updateOrder(
          orderId,
          ApiOrderUpdateRequest(
              contactEmail: "lolo@mail.com",
              customerEmail: "hoho@gmail.com",
              customerPhone: null,
              customerFirstName: "Lolo",
              customerLastName: "Hoho",
              note: "MONKEY ORDER",
              state: ApiOrderState.productsNotScanned));
      expect(updateOrderResult.isRight, isTrue);

      final getOrderResult = await orderService.getOrderById(orderId);

      expect(getOrderResult.isRight, isTrue);
      expect(getOrderResult.right.contactEmail, "lolo@mail.com");
      expect(getOrderResult.right.customerEmail, "hoho@gmail.com");
      expect(getOrderResult.right.customerPhone, isNull);
      expect(getOrderResult.right.note, "MONKEY ORDER");
      expect(getOrderResult.right.customerFirstName, "Lolo");
      expect(getOrderResult.right.customerLastName, "Hoho");
    });

    test('Create and search order', () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final orderService = OrderService(api);
      String randomFirstName = getRandomString(10);
      String randomLastName = getRandomString(10);
      String randomEmail = "${getRandomString(10)}@mail.com";
      String randomPhone = "+336${getRandomString(7)}";
      String randomNote = getRandomString(10);

      final createOrderResult = await orderService.createOrder(
        ApiOrderCreate(
            contactEmail: randomEmail,
            customerEmail: randomEmail,
            customerPhone: randomPhone,
            customerFirstName: randomFirstName,
            customerLastName: randomLastName,
            note: randomNote),
      );
      expect(createOrderResult.isRight, isTrue);

      var result =
          await orderService.searchOrders(randomNote, 0, DateTime.now());
      expect(result.isRight, isTrue);
      expect(result.right.length, 1);

      result =
          await orderService.searchOrders(randomFirstName, 0, DateTime.now());
      expect(result.isRight, isTrue);
      expect(result.right.length, 1);

      result =
          await orderService.searchOrders(randomLastName, 0, DateTime.now());
      expect(result.isRight, isTrue);
      expect(result.right.length, 1);

      result = await orderService.searchOrders(randomEmail, 0, DateTime.now());
      expect(result.isRight, isTrue);
      expect(result.right.length, 1);

      result = await orderService.searchOrders(randomPhone, 0, DateTime.now());
      expect(result.isRight, isTrue);
      expect(result.right.length, 1);

      result = await orderService.searchOrders(randomNote, 0, DateTime.now());
      expect(result.isRight, isTrue);
      expect(result.right.length, 1);
    });
  });
}
