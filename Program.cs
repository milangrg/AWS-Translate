using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.Translate;
using Amazon.Translate.Model;

namespace TranslateClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            bool exit = false;

            AWSCredentials credentials = GetAWSCredentialsByName("default");

            AmazonTranslateClient translateClient = new AmazonTranslateClient(credentials, RegionEndpoint.USEast1);

            List<TargetLanguage> targetLanguages = new List<TargetLanguage>();
            LoadListWithTargetLanguages(targetLanguages);
            

            while (!exit)
            {
                Console.Write("Enter a sentence (or q to quit): ");
                string userInput = Console.ReadLine();

                if (userInput == "q" || userInput == "quit")
                {
                    exit = true;
                } 
                else
                {
                    foreach (TargetLanguage language in targetLanguages)
                    {
                        TranslateTextRequest request = new TranslateTextRequest
                        {
                            Text = userInput,
                            SourceLanguageCode = "en",
                            TargetLanguageCode = language.languageCode
                        };
                        Task<TranslateTextResponse> result = translateClient.TranslateTextAsync(request);
                        Console.WriteLine(language.languageName + ": " + result.Result.TranslatedText);
                    }
                    Console.WriteLine();
                }
            }

        }

        private static AWSCredentials GetAWSCredentialsByName(string profileName)
        {
            if (String.IsNullOrEmpty(profileName))
            {
                throw new ArgumentNullException("profileName cannot be null or empty");
            }

            SharedCredentialsFile credFile = new SharedCredentialsFile();
            CredentialProfile profile = credFile.ListProfiles().Find(p => p.Name.Equals(profileName));
            if (profile == null)
            {
                throw new Exception(String.Format("Profile named {0} not found", profileName));
            }

            return AWSCredentialsFactory.GetAWSCredentials(profile, new SharedCredentialsFile());
        }

        public static void LoadListWithTargetLanguages(List<TargetLanguage> list)
        {
            list.Add(new TargetLanguage { languageName = "DANISH", languageCode = "da" });
            list.Add(new TargetLanguage { languageName = "ITALIAN", languageCode = "it" });
            list.Add(new TargetLanguage { languageName = "FRENCH", languageCode = "fr" });
            list.Add(new TargetLanguage { languageName = "SPANISH", languageCode = "es" });
        }
    }
}