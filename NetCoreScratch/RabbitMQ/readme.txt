Setup Docker with RabbotMQ
--------------------------

1) docker run -d --hostname rmq --name rabbitmq -p 8080:15672 -p 5672:5672 rabbitmq:3-management
2) http://localhost:8080/
3) guest/guest

Execute
-------

1) Set multiple startup projects in Visual Studio
	* RabbitSender
	* RabbitReceiver1
	* RabbitReceiver2

2) Start