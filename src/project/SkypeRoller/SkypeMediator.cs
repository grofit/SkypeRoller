using System;
using System.Windows.Forms;
using SKYPE4COMLib;
using core.roller;

namespace SkypeRoller
{
    public class SkypeMediator
    {
        private const int FiveSeconds = (1000 * 5);
        
        private bool havingBindingProblems = true;
        private Timer bindingCheckTimer;

        private Skype skype;

        private DiceOutputGenerator diceOutputGenerator;
        private RollGenerator rollGenerator;
        private DiceCommandParser diceCommandParser;

        private RequestOutputGenerator requestOutputGenerator;
        private RequestCommandParser requestCommandParser;
        private VersionCommandParser versionCommandParser;

        public SkypeMediator()
        {
            diceOutputGenerator = new DiceOutputGenerator(new TotalScoreGenerator());
            rollGenerator = new RollGenerator();
            diceCommandParser = new DiceCommandParser();
            versionCommandParser = new VersionCommandParser();

            requestOutputGenerator = new RequestOutputGenerator();
            requestCommandParser = new RequestCommandParser();

            CreateBindingCheckTimer();
        }

        public void StartMediation()
        {
            StartSkypeAndHooks();
            BindToSkype();
            CheckBindingStatus();
        }

        public void StopMediation()
        {
            bindingCheckTimer.Dispose();
        }

        protected void StartSkypeAndHooks()
        {
            try { skype = new Skype(); }
            catch (Exception ex)
            {
                var title = "Skype Binding Error";
                var message = "Unable to use Skype4Com component, this could be due to Extras Manager not being available or Skype not configured for addons. Do you want to see debug output?";
                if(MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                { MessageBox.Show(ex.ToString(), "Debug Information", MessageBoxButtons.OK); }
                Program.RollerTrayForm.CloseApplication();
            }

            if (!skype.Client.IsRunning)
            { skype.Client.Start(); }

            skype.MessageStatus += skype_MessageStatus;
        }

        protected void BindToSkype()
        {
            if ((skype as ISkype).AttachmentStatus != TAttachmentStatus.apiAttachSuccess)
            { skype.Attach(); }
        }

        private void skype_MessageStatus(ChatMessage message, TChatMessageStatus status)
        {
            if (status == TChatMessageStatus.cmsSending)
            {
                CheckForDiceCommands(message);
                CheckForVersionCommands(message);
                CheckForRequestCommands(message);
            }
            
            if(status == TChatMessageStatus.cmsReceived)
            {
                CheckForRequestCommands(message);
            }
        }

        private void CheckForDiceCommands(ChatMessage message)
        {
            if (diceCommandParser.IsMatchingCommand(message.Body))
                {
                    var diceCommand = diceCommandParser.ParseCommandFromMessage(message.Body, message.FromDisplayName);
                    var diceResults = rollGenerator.GenerateRollResults(diceCommand);
                    var outputMessage = diceOutputGenerator.GenerateOutputMessage(diceResults);
                    message.Chat.SendMessage(outputMessage);
                }
        }

        private void CheckForVersionCommands(ChatMessage message)
        {
            if (versionCommandParser.IsMatchingCommand(message.Body))
            {
                var currentVersion = GetType().Assembly.GetName().Version;
                var versionInfo = string.Format("Currently Running SkypeRoller Version: {0}", currentVersion);
                message.Chat.SendMessage(versionInfo);
            }
        }

        private void CheckForRequestCommands(ChatMessage message)
        {
            if (requestCommandParser.IsMatchingCommand(message.Body))
            {
                var requestCommand = requestCommandParser.ParseCommandFromMessage(message.Body, message.FromDisplayName);
                var outputMessage = requestOutputGenerator.GenerateOutputMessage(requestCommand);
                Program.RollerTrayForm.DisplayInfoMessage("Interaction Requested", outputMessage);
            }
        }

        protected void CreateBindingCheckTimer()
        {
            bindingCheckTimer = new Timer();
            bindingCheckTimer.Interval = FiveSeconds;
            bindingCheckTimer.Tick += timer_CheckBindingStatus;
            bindingCheckTimer.Start();
        }

        protected void CheckBindingStatus()
        {
            if (havingBindingProblems)
            {
                if ((skype as ISkype).AttachmentStatus == TAttachmentStatus.apiAttachSuccess)
                {
                    Program.RollerTrayForm.DisplayBindingSuccessMessage();
                    havingBindingProblems = false;
                }
            }

            if ((skype as ISkype).AttachmentStatus != TAttachmentStatus.apiAttachSuccess)
            {
                Program.RollerTrayForm.DisplayBindingProblemMessage();
                BindToSkype();
                havingBindingProblems = true;
            }
        }

        protected void timer_CheckBindingStatus(object sender, EventArgs eventArgs)
        {
            CheckBindingStatus();
        }
    }
}
