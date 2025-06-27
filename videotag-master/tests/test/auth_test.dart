import 'package:cookie_jar/cookie_jar.dart';
import 'package:tests/config.dart';
import 'package:test/test.dart';
import 'package:marycard_client/marycard_client.dart';
import 'package:dio_cookie_manager/dio_cookie_manager.dart';
import 'package:tests/helper.dart';

void main() {
  group("Login", () {
    test('Sign in with admin', () async {
      var api = await Api.create(Config(apiUrl: url));
      var auth = AuthService(api);

      var result = await auth.signIn(
          ApiSignInRequest(identifier: adminUsername, password: adminPassword));
      expect(result.isRight, isTrue);
      expect(result.right, isNotNull);
      expect(result.right, isA<ApiSignInResponse>());
      expect(result.right.signedIn, isTrue);
    });

    test('Check Auth Anonymous', () async {
      var api = await Api.create(Config(apiUrl: url));
      var auth = AuthService(api);

      var result = await auth.getConnectedUser();
      expect(result.isLeft, isTrue);
    });

    test('Check Auth Admin', () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      var auth = AuthService(api);

      final authResult = await auth.getConnectedUser();
      expect(authResult.isRight, isTrue);
    });
  });
}
