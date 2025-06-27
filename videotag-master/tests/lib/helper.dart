import 'dart:math';

import 'package:marycard_client/marycard_client.dart';
import 'package:tests/config.dart';

Future<ApiSignInResponse> connectAdmin(Api api) async {
  final auth = AuthService(api);

  final result = await auth.signIn(
      ApiSignInRequest(identifier: adminUsername, password: adminPassword));

  assert(result.isRight);
  assert(result.right.signedIn);
  return result.right;
}

const _chars = 'AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz1234567890';
Random _rnd = Random();

String getRandomString(int length) => String.fromCharCodes(Iterable.generate(
    length, (_) => _chars.codeUnitAt(_rnd.nextInt(_chars.length))));

String randomUid() {
  return "test_${getRandomString(15)}";
}
