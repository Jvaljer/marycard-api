import 'package:cookie_jar/cookie_jar.dart';
import 'package:tests/config.dart';
import 'package:test/test.dart';
import 'package:marycard_client/marycard_client.dart';
import 'package:dio_cookie_manager/dio_cookie_manager.dart';

void main() {
  group("GET", () {
    test('Get connected with admin', () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      var auth = AuthService(api);

      var result = await auth.signIn(
          ApiSignInRequest(identifier: adminUsername, password: adminPassword));
      expect(result.right.signedIn, isTrue);

      var connected = await auth.getConnectedUser();
      expect(connected.right.identifier, adminUsername);
    });

    test('Get admin by ID', () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      var auth = AuthService(api);

      var result = await auth.signIn(
          ApiSignInRequest(identifier: adminUsername, password: adminPassword));
      expect(result.right.signedIn, isTrue);

      var connected = await auth.getConnectedUser();
      expect(connected.right.identifier, adminUsername);

      var user = await auth.getUser(connected.right.id);
      expect(connected.right, user.right);
    });
  });
}
