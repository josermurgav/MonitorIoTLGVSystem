# Sistema IoT de monitoreo de estado en tiempo Real de flota de LGV

##  Aplicación escrita en C#, librerias RabbitMQ Client 6.8.1, Newtonsoft.Json 13.03, Microsoft.Azure.Devices.Client 1.42.3

Este proyecto es una propuesta de implementación de un sistema IoT para la recolección de información de estado y monitoreo de flotas de vehículos LGV. Implementa:
Clase LGV_Device: simula un LGV que envía sus datos de estado a un nodo central en el sistema a través de un protocolo AMQP implementado con RabbitMQ
Clase MonitorLGVSystem: implementa el nodo central en el sistema que recibe los datos de los LGV a través del protocolo AQMP y envía estos datos a la nube a través de conexiones seguras con IoT Hub.
Además, esta clase de encarga de calcular tiempos de eficiencia y funcionamiento de los LGV.
