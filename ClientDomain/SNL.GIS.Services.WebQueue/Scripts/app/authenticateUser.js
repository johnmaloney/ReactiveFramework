var UserAuthentication = function (hub) {
	console.log("Found Authentication method.");


	this.guid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
		var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
		return v.toString(16);
	});
}

UserAuthentication.prototype = {

	login: function () {
		console.log("User Login Command executed.");
		var hub = $.connection.authenticationHub;
		hub.server.login(this.guid);
	}



}