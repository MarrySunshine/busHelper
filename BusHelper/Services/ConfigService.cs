namespace BusHelper.Services
{
    public class ConfigService
    {
        static private ConfigService instance = null;
        static public ConfigService Instance
        {
            get
            {
                return instance == null ? (instance = new ConfigService()) : instance;
            }
        }

        public string apikey = "5034722f207182d020737526c9b8b9dc";
        public string interfaceUrl = "http://apis.baidu.com/xiaota/bus_lines/buses_lines";
        public enum RequestStatusCode: int
        {
            Success = 1000,
            HaveNoBus = 1001,
            HaveNoMatchBus = 1002,
            HaveNoCity = 1003,
            ParameterError = 9999
        }
    }
}
