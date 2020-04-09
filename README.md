# Covid 19 server

### Architecture

![alt text](https://github.com/CovidWorld/server/blob/master/Images/Covid%2019%20server%20architecture.png "Architecture")

### Configuration
#### API    
    "CosmosEndpoint": Cosmos DB URL
    "CosmosAuthKey": Cosmos DB Auth Key
    "CosmosDatabase": Cosmos DB Database name

    "AutoNumberStorageConnection": Connection string to azure storage account where autonumber library store file with last used sequence uid
    "AutoNumberContainerName": Name of the Blob Container for autonumber library in storage account above
    "AutoNumberBatchSize": Autonumber batch size. More details https://itnext.io/generate-auto-increment-id-on-azure-62cc962b6fa6
    "AutoNumberMaxWriteAttempts":  Autonumber max write attemts

    "FirebaseProjectId": Project id from firebase consele. Using for auth on some endpoints
    "FirebaseUrl": Needed for sending notification to user app
    "FirebaseServerKey": Needed for sending notification to user app https://developer.clevertap.com/docs/find-your-fcm-sender-id-fcm-server-api-key
    "FirebaseSenderId": Needed for sending notification to user app https://developer.clevertap.com/docs/find-your-fcm-sender-id-fcm-server-api-key

    "TwilioAccountSid": SMS provider setting https://support.twilio.com/hc/en-us/articles/223136607-What-is-an-Application-SID-
    "TwilioAuthToken": SMS provider setting https://support.twilio.com/hc/en-us/articles/223136027-Auth-Tokens-and-How-to-Change-Them
    "TwilioPhoneNumber":  SMS provider setting

    "MfaTokenGeneratorSecret": Secret for OTP Token generator

    "ConfirmInfectionNotificationEnabled": Enable or disable sending notification to all users whose was in contact wit other user that designate yourself as infected in app

    "MedicalIdHashSalt": Salt used for generating medical hash. https://hashids.org/. Medical hash is for anonymous user identification by ministry of healt etc.
    "MedicalIdHashMinValue": Setting of medical hash algoritm
    "MedicalIdHashAlphabet": Setting of medical hash algoritm
    
