using MQTTnet.Client;
using MQTTnet.Diagnostics;
using MQTTnet.Implementations;

namespace Lab4Activity3;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var mqttService = new MqttService(new MqttClient(new MqttClientAdapterFactory(), new MqttNetEventLogger()));
        await mqttService.ConnectToMqttBrokerAsync();

        var thermostat = new Thermostat(mqttService);
        await thermostat.StartSimulationAsync();
    }
}