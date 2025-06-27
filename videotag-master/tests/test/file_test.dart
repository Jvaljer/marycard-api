import 'dart:io';

import 'package:cookie_jar/cookie_jar.dart';
import 'package:dio_cookie_manager/dio_cookie_manager.dart';
import 'package:marycard_client/marycard_client.dart';
import 'package:test/test.dart';
import 'package:tests/config.dart';
import 'package:tests/helper.dart';

void main() {
  group('File upload', () {
    test("Upload file with authenticated user", () async {
      var api = await Api.create(Config(apiUrl: url),
          cookieManager: CookieManager(CookieJar()));
      await connectAdmin(api);

      final fileService = FileService(api);

      final file = await fileService.uploadFile(File("./templates/monkey.png"));
      expect(file.isRight, isTrue);
    });

    test("Upload file with unauthenticated user", () async {
      var api = await Api.create(Config(apiUrl: url));
      final fileService = FileService(api);

      final file = await fileService.uploadFile(File("./templates/monkey.png"));
      expect(file.isLeft, isTrue);
    });
  });
}
