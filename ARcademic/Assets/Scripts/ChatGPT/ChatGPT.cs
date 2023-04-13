using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        public Button button;
        public List<string> voskOutputFileNames;
        public List<string> tesseractOutputFileNames;
        public string texOutputFileName;
        public string htmlOutputFileName;

        private OpenAIApi openai = new OpenAIApi();
        private string startPrompt = "Organize the following text into concise, LaTeX formatted notes. The notes should be in a list " +
            "if appropriate. Also, fix any spelling or grammatical errors. Be sure to include a section header and the proper document " +
            "begin and end tags. Use the article document class and the graphicx package. Separate the notes into multiple sections " +
            "that best organize all of the information. Each individual note in itemized lists should be concise without caring about " +
            "grammar. Do not include repetitive information. Do not type anything else after or before the Latex:";
        private string endPrompt = "I will give you two LaTeX formatted notes for class. Merge them into one LaTeX document, " +
            "organizing the information in a logical manner. Do not repeat information.";
        private string htmlPrompt = "Make the following Latex into HTML.Be sure to include tags for a proper HTML site, such " +
            "as doctype and body. Have the <title> tag be 'ARcademic Notes'. Include basic css " +
            "to make the text look like good, readable notes. Do not type anything before or after the HTML:";

        public async void GenerateLatex(List<string> tesseractFileNames, List<string> voskFileNames)
        {
            List<string> fileNames = tesseractFileNames;
            foreach (string voskFile in voskFileNames)
            {
                fileNames.Add(voskFile);
            }

            /****************************** CONVERT EACH FILE INTO LATEX ******************************/
            List<string> latexOutputs = new List<string>();
            List<ChatMessage> messages = new List<ChatMessage>();
            foreach (string file in fileNames)
            {
                string textResponse = "";

                // Get the text from the files
                string textData = FileIO.ReadString(file);

                // Create the message to send to GPT
                var newMessage = new ChatMessage();
                newMessage.Role = "user";
                newMessage.Content = startPrompt + "\n\"" + textData + "\"";

                messages.Add(newMessage);

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
                latexOutputs.Add(textResponse);
                messages.Remove(newMessage);
            }
            /***************************************************************************/


            /****************************** MERGE THE FIRST TWO LATEX FILES ******************************/
            var merged = "";
            if (latexOutputs.Count > 1)
            {
                var newMessage = new ChatMessage();
                newMessage.Role = "user";
                newMessage.Content = endPrompt + "\nDocument 1:\n" + latexOutputs[0] + "\n\nDocument 2:\n" + latexOutputs[1];

                messages.Add(newMessage);

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
                    merged = message.Content;
                }
                else
                {
                    Debug.LogWarning("No text was generated from this prompt.");
                }
                messages.Remove(newMessage);
            } 
            else
            {
                merged = latexOutputs[0];
            }
            /***************************************************************************/


            /****************************** MERGE THE REMAINING LATEX FILES ******************************/
            for (int i = 2; i < latexOutputs.Count; i++)
            {
                var newMessage = new ChatMessage();
                newMessage.Role = "user";
                newMessage.Content = endPrompt + "\nDocument 1:\n" + merged + "\n\nDocument 2:\n" + latexOutputs[i];

                messages.Add(newMessage);

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
                    merged = message.Content;
                }
                else
                {
                    Debug.LogWarning("No text was generated from this prompt.");
                }
                messages.Remove(newMessage);
            }
            /***************************************************************************/

            /****************************** CONVERT LATEX TO HTML ******************************/
            string html = "";
            for (int i = 0; i < 1; i ++)
            {
                var newMessage = new ChatMessage();
                newMessage.Role = "user";
                newMessage.Content = htmlPrompt + "\n\n" + merged;

                messages.Add(newMessage);

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
                    html = message.Content;
                }
                else
                {
                    Debug.LogWarning("No text was generated from this prompt.");
                }
                messages.Remove(newMessage);
            }
            /***************************************************************************/

            /****************************** WRITE TO FILES ******************************/
            FileIO.WriteString(merged, texOutputFileName);
            FileIO.WriteString(html, htmlOutputFileName);
            /***************************************************************************/
        }

        // Button listener can't have parameters. This is purely for testing and will be removed in final product.
        public void CallConvertFromButton()
        {
            GenerateLatex(tesseractOutputFileNames, voskOutputFileNames);
        }

        public void OpenHTMLFile()
        {
            Application.OpenURL(Application.persistentDataPath + "/" + htmlOutputFileName);
        }
    }
}
