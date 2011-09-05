namespace core.roller
{
    public class RequestOutputGenerator
    {
        public string GenerateOutputMessage(RequestCommand requestCommand)
        {
            return string.Format("{0} has requested: {1}", requestCommand.Requester, requestCommand.RequestMessage);
        }
    }
}