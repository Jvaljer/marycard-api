import 'dart:io';

import 'package:tests/config.dart';

Future<String> uploadSample(
    {String dirPath = "python",
    String apiKey = videoApiKey,
    String uploadToken = videoUploadToken,
    String filePath = "../sample.mp4"}) async {
  final process = await Process.start(
    'bash',
    [
      '-c',
      'cd $dirPath && source venv/bin/activate && python3 upload.py $apiKey $uploadToken $filePath'
    ],
    runInShell: true,
  );

  final output = StringBuffer();
  final errorOutput = StringBuffer();

  process.stdout.transform(SystemEncoding().decoder).listen(output.write);
  process.stderr.transform(SystemEncoding().decoder).listen(errorOutput.write);

  final exitCode = await process.exitCode;
  if (exitCode != 0) {
    throw Exception('Process exited with code $exitCode: $errorOutput');
  }

  return output.toString().trim();
}
