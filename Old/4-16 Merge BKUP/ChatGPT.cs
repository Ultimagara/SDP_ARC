using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] private Button button; // TODO: Remove for final product

        private OpenAIApi openai = new OpenAIApi();
        private string startPrompt = "Organize the following text into concise, LaTeX formatted notes. The notes should be in a list " +
            "if appropriate. Also, fix any spelling or grammatical errors. Be sure to include a section header and the proper document " +
            "begin and end tags. Use the article document class and the graphicx package. Separate the notes into multiple sections " +
            "that best organize all of the information. Each individual note in itemized lists should be concise without caring about " +
            "grammar. Do not include repetitive information. Do not type anything else after or before the Latex:";
        private string endPrompt = "I will give you two LaTeX formatted notes for class. Merge them into one LaTeX document, " +
            "organizing the information in a logical manner. Do not repeat information.";

        public async void ConvertTextFilesToLatex(string textFileName, string speechFileName, string outputFileName)
        {
            string textResponse = "";
            string speechResponse = "";

            // Get the text from the files
            string textData = FileIO.ReadString(textFileName);
            string speechData = FileIO.ReadString(speechFileName);

            // Create the first message to send to GPT
            List<ChatMessage> messages = new List<ChatMessage>();
            var newMessage = new ChatMessage();
            newMessage.Role = "user";
            newMessage.Content = startPrompt + "\n\"" + textData + "\"";

            messages.Add(newMessage);

            Debug.Log("sending first message");
            // Send the message and await a response
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                textResponse = message.Content;
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            messages.Remove(newMessage);

            Debug.Log("sending second message");
            // Create the second message to send to GPT
            newMessage = new ChatMessage();
            newMessage.Role = "user";
            newMessage.Content = startPrompt + "\n\"" + speechData + "\"";

            messages.Add(newMessage);

            // Send the message and await a response
            completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                //FileIO.WriteString(message.Content, outputFileName);
                speechResponse = message.Content;
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }

            messages.Remove(newMessage);

            // Create the final message to send to GPT
            newMessage = new ChatMessage();
            newMessage.Role = "user";
            newMessage.Content = endPrompt + "\nDocument 1:\n" + textResponse + "\n\nDocument 2:\n" + speechResponse;

            messages.Add(newMessage);

            Debug.Log("sending final message");
            // Send the message and await a response
            completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0301",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();
                FileIO.WriteString(message.Content, outputFileName);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }
        }

        private void Start()
        {
            button.onClick.AddListener(CallConvertFromButton);
        }

        // Button listener can't have parameters. This is purely for testing and will be removed in final product.
        private void CallConvertFromButton()
        {
            ConvertTextFilesToLatex("testTextData.txt", "testSpeechData.txt", "output.tex");
        }
    }
}
