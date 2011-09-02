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

        private RollOutputGenerator rollOutputGenerator;
        private RollGenerator rollGenerator;
        private MessageParser messageParser;

        public SkypeMediator()
        {
            rollOutputGenerator = new RollOutputGenerator(new TotalScoreGenerator());
            rollGenerator = new RollGenerator();
            messageParser = new MessageParser();

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
                if (messageParser.IsValidRollCommand(message.Body))
                {
                    var diceCommand = messageParser.ParseCommandFromMessage(message.Body, message.FromDisplayName);
                    var diceResults = rollGenerator.GenerateRollResults(diceCommand);
                    var outputMessage = rollOutputGenerator.GenerateOutputMessage(diceResults);
                    message.Chat.SendMessage(outputMessage);
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
