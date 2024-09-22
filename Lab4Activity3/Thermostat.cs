namespace Lab4Activity3;

public class Thermostat(MqttService mqttService)
{
    private const int TemperatureMin = 15;
    private const int TemperatureMax = 30;
    private const int IdealTemperature = 22;
    private const int SleepDurationMs = 2000;

    private readonly Random _random = new();

    public async Task StartSimulationAsync()
    {
        while (true)
        {
            int currentTemperature = GenerateRandomTemperature();
            string state = CalculateThermostatState(currentTemperature);

            await mqttService.PublishTemperatureDataAsync(currentTemperature, IdealTemperature, state);
            Thread.Sleep(SleepDurationMs);
        }
    }

    private int GenerateRandomTemperature()
    {
        return _random.Next(TemperatureMin, TemperatureMax);
    }

    private string CalculateThermostatState(int currentTemp)
    {
        if (currentTemp < IdealTemperature)
        {
            return "heating";
        }
        if (currentTemp > IdealTemperature)
        {
            return "cooling";
        }
        return "idle";
    }
}