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

        public SkypeMediator()
        {
            diceOutputGenerator = new DiceOutputGenerator(new TotalScoreGenerator());
            rollGenerator = new RollGenerator();
            diceCommandParser = new DiceCommandParser();

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
            skype = new Skype();
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
                if (diceCommandParser.IsMatchingCommand(message.Body))
                {
                    var diceCommand = diceCommandParser.ParseCommandFromMessage(message.Body, message.FromDisplayName);
                    var diceResults = rollGenerator.GenerateRollResults(diceCommand);
                    var outputMessage = diceOutputGenerator.GenerateOutputMessage(diceResults);
                    message.Chat.SendMessage(outputMessage);
                }
            }
            
            if(status == TChatMessageStatus.cmsReceived)
            {
                if(requestCommandParser.IsMatchingCommand(message.Body))
                {
                    var requestCommand = requestCommandParser.ParseCommandFromMessage(message.Body, message.FromDisplayName);
                    var outputMessage = requestOutputGenerator.GenerateOutputMessage(requestCommand);
                    Program.RollerTrayForm.DisplayInfoMessage("Interaction Requested", outputMessage);
                }
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
