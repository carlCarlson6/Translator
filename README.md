# translator

![schema](/docs/translator.png)

## to run the project on locahost
the following configurations are mandatory to be present on your dotnet secret.json or on appsettings.json
```
{
  "StorageAccountConfiguration": {
    "ConnectionString": "<STORAGE_ACCOUNT_CONNECTION_STRING>"
  },
  "AzureCognitiveServicesConfig": {
    "TranslatorUrl": "<URL>",
    "TranslatorApiKey": "<API_KEY>",
    "TranslatorRegion": "<REGION>"
  },
  "RebusQueueSettings": {
    "TranslatorWebAppQueue": "<QUEUE_NAME>"
  }
}
```

on the docs folder there is a postman collection to test the endpoints

## to run the project on docker-compose
WIP

## to run the tests
to be able to run the test suite you will need to have [docker](https://www.docker.com/) installed and it's deamon running on background
the project also contains a github action yml file, so you will be able to build and test the project on the a github repo