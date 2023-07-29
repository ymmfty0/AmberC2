namespace Client.Services.Api
{
    public static class ApiList
    {
        //Listeners
        public readonly static string GET_LISTENERS = "/api/listeners";
        public readonly static string GET_LISTENER_BY_ID = "/api/listeners/";
        public readonly static string DELETE_LISTENER_BY_ID = "/api/listeners/";
        public readonly static string CREATE_LISTENER = "/api/listeners";

        //Listeners
        public readonly static string GET_LISTENER_TYPES = "/api/listeners/type";
        public readonly static string GET_LISTENE_TYPE_BY_ID = "/api/listeners/type/";
        public readonly static string DELETE_LISTENER_TYPE_BY_ID = "/api/listeners/type/";
        public readonly static string CREATE_LISTENER_TYPE = "/api/listeners/type/";

        //Agents
        public readonly static string GET_AGENTS = "/api/agents";
        public readonly static string GET_AGENT_BY_ID = "/api/agents/";
        public readonly static string DELETE_AGENT_BY_ID = "/api/agents/";
        public readonly static string CREATE_AGENT = "/api/agents/";

        //Implant
        public readonly static string GET_IMPLANTS = "/api/implants";
        public readonly static string GET_IMPLANT_BY_ID = "/api/implants/";
        public readonly static string DELETE_AGENTS_BY_ID = "/api/implants/";
        public readonly static string CREATE_AGENTS_BY = "/api/implants/";



        //Agents
        public readonly static string GET_AGENT_TASKS = "/api/agents/task";
        public readonly static string GET_AGENT_TASK_BY_ID = "/api/agents/task/";
        public readonly static string DELETE_AGENT_TAKS_BY_ID = "/api/agents/task/";
        public readonly static string CREATE_AGENT_TASK = "/api/agents/task/";

        public readonly static string GET_AGENT_TASK_RESULTS = "/api/agent/task/result";
        public readonly static string GET_AGENT_TASK_RESULT_BY_ID = "/api/agent/task/result/";
        //public readonly static string DELETE_AGENT_TAKS_BY_ID = "/api/agent/task/result//";


    }
}
