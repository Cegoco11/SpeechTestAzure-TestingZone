using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTestAzure
{
    class Program
    {
        static void Main(string[] args)
        {
            var recognizer = RecognizeSpeechAsync();
            recognizer.StartContinuousRecognitionAsync();

            Console.WriteLine("Press any key to exit.");

            Console.ReadLine();

            //var config = SpeechConfig.FromSubscription("86973d6d8c0f465fb280110704a3409c", "westus");
            //config.SpeechSynthesisLanguage = "es-ES";
            //config.SpeechSynthesisVoiceName = "es-ES-HelenaRUS";
            //config.SpeechSynthesisVoiceName = "es-ES-Pablo-Apollo";
            //var cortana = new SpeechSynthesizer(config);
            //var text = "Hello world!";
            //var result = cortana.SpeakTextAsync(text);


            SynthesisToAudioFileAsync().Wait();

            Console.WriteLine("Press any key to exit.");

            Console.ReadLine();
        }


        #region Reconocimiento

        public static SpeechRecognizer RecognizeSpeechAsync()
        {
            var config = SpeechConfig.FromSubscription("86973d6d8c0f465fb280110704a3409c", "westus");
            config.SpeechRecognitionLanguage = "es-ES";

            var recognizer = new SpeechRecognizer(config);
            recognizer.Recognized += LogResult;
            recognizer.SpeechStartDetected += LogStart;

            return recognizer;

        }
        static void LogResult(object sender, SpeechRecognitionEventArgs e)
        {
            Console.WriteLine("## " + e.Result.Text);
            Console.WriteLine("# Duracion: " + e.Result.Duration);
            Console.WriteLine("");
        }

        static void LogStart(object sender, RecognitionEventArgs e)
        {
            Console.WriteLine("Comenzando analisis");
        }

        #endregion


        public static async Task SynthesisToAudioFileAsync()
        {
            var config = SpeechConfig.FromSubscription("b7f8752c5aaf46729b1dd7eb05851559", "westus");
            var fileName = "helloworld.wav";
            using (var fileOutput = AudioConfig.FromWavFileOutput(fileName))
            {
                using (var synthesizer = new SpeechSynthesizer(config))
                {
                    var text = "Hello world!";
                    var result = await synthesizer.SpeakTextAsync(text);

                    if (result.Reason == ResultReason.SynthesizingAudioCompleted)
                    {
                        Console.WriteLine($"Speech synthesized for text [{text}]");
                    }
                    else if (result.Reason == ResultReason.Canceled)
                    {
                        var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                        Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                        if (cancellation.Reason == CancellationReason.Error)
                        {
                            Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                            Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                            Console.WriteLine($"CANCELED: Did you update the subscription info?");
                        }
                    }
                }
            }
        }

    }
}


