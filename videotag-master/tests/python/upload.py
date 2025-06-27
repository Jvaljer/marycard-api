import apivideo
from apivideo.api import videos_api
from apivideo.configuration import Configuration
import argparse
import sys

def main():
    parser = argparse.ArgumentParser(description='Upload a video using api.video.')
    parser.add_argument('api_key', type=str, help='API key for authentication')
    parser.add_argument('upload_token', type=str, help='Upload token for the video')
    parser.add_argument('file_path', type=str, help='Path to the video file')

    args = parser.parse_args()

    configuration = Configuration(chunk_size=10 * 1024 * 1024)
    with apivideo.AuthenticatedApiClient(args.api_key, configuration=configuration) as api_client:
        api_instance = videos_api.VideosApi(api_client)
        file = open(args.file_path, 'rb')

        try:
            api_response = api_instance.upload_with_upload_token(args.upload_token, file)
            print(api_response["video_id"])
            sys.exit(0)
        except apivideo.ApiException as e:
            print("Exception when calling VideosApi->upload_with_upload_token: %s\n" % e)
            sys.exit(1)

if __name__ == "__main__":
    main()