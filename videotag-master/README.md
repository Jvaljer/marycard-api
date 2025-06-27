# videotag

## Localhost

To run the backend locally, you need to have to setup an environment with the following :
* [azurite](https://github.com/Azure/Azurite)
* [mssql](https://hub.docker.com/r/microsoft/mssql-server)

Once you have those running, you must change the appsettings.Development.json file to match your local environment.

Then, build the migrator project and run the migrations.

```sh
dotnet build Doctor --configuration Release -warnaserror -o build-doctor
./build-doctor/Doctor -f "./yourpath/appsettings.Development.json" -i
```

Now you should be able to run the backend locally.

## Test

Test are written using Dart.
Setup the runtime and then run:

```sh
cd tests/python
python3 -m venv venv
source venv/bin/activate
pip3 install -r requirements.txt
cd ..
dart test
```

### Adding tests

To add tests, modify `marycard-client-dart` on the dev branch if needed.
Then, to force dart to fetch `dev` again, run:

```sh
dart pub upgrade
```
