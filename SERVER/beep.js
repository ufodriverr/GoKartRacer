var io = require('C:/Program Files/nodejs/node_modules/npm/node_modules/socket.io')({
	transports: ['websocket'],
});

io.attach(4567);

io.on('connection', function(socket){
	socket.on('beep', function(){
		socket.emit('boop');
	});
})

console.log('Server started');