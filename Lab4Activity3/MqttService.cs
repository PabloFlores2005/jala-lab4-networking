using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace Lab4Activity3;

public class MqttService(IMqttClient mqttClient)
{
        private const string MqttBrokerAddress = "localhost";
        private const string MqttTopic = "home/thermostat";

        public async Task ConnectToMqttBrokerAsync()
        {
            var mqttOptions = BuildMqttClientOptions();
            mqttClient = new MqttFactory().CreateMqttClient();

            mqttClient.DisconnectedAsync += async e =>
            {
                Console.WriteLine("Disconnected from MQTT broker. Attempting to reconnect...");
                await ReconnectToMqttBrokerAsync(mqttOptions);
            };

            await mqttClient.ConnectAsync(mqttOptions);
            Console.WriteLine($"Connected to MQTT broker at {MqttBrokerAddress}");
        }

        public async Task PublishTemperatureDataAsync(int currentTemp, int idealTemp, string state)
        {
            var message = CreateTemperatureMessage(currentTemp, idealTemp, state);

            var mqttMessage = new MqttApplicationMessageBuilder()
                .WithTopic(MqttTopic)
                .WithPayload(Encoding.UTF8.GetBytes(message))
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
                .Build();

            await mqttClient.PublishAsync(mqttMessage);
            Console.WriteLine($"Published: {message} to topic '{MqttTopic}'");
        }

        private string CreateTemperatureMessage(int currentTemp, int idealTemp, string state)
        {
            return $"Temperature: {currentTemp}°C, Ideal: {idealTemp}°C, State: {state}";
        }

        private MqttClientOptions BuildMqttClientOptions()
        {
            return new MqttClientOptionsBuilder()
                .WithTcpServer(MqttBrokerAddress)
                .Build();
        }

        private async Task ReconnectToMqttBrokerAsync(MqttClientOptions options)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            try
            {
                await mqttClient.ConnectAsync(options);
                Console.WriteLine("Reconnected to MQTT broker.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Reconnection failed: {ex.Message}");
            }
        }

    }