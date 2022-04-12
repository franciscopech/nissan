var cookie_name = "products-shop";
var unidades_contenedor = 3;
function Invoke(JsonData, _url, callback = null, StatusError = null, okenable=false, error_callback = null)
{
	$.ajax({
			async:true,
			url: "/" + _url + "/",
			datatype:'json',
			type:'POST',
			contentType: "application/json",
			//headers:{ "X-CSRFToken": Cookies.get('csrftoken') },
			data:JSON.stringify(JsonData),
			success:function(data)
			{
				if(callback != null && data.Code == 0)
					callback(data);
				
				if(StatusError != null && data.Code != 0)
					StatusError(data)
			},
			error:function(err)
			{
				if(error_callback != null)
					error_callback(err);
			}
		});
}
function InvokeObject(JsonData, _url, callback = null, StatusError = null, okenable = false, error_callback = null) {
	$.ajax({
		async: true,
		url: "/" + _url + "/",
		datatype: 'json',
		type: 'POST',
		contentType: "application/json",
		data: JSON.stringify(JsonData),
		success: function (data) {
			if (callback != null && data.RequestStatus.Code == 0)
				callback(data);

			if (StatusError != null && data.RequestStatus.Code != 0)
				StatusError(data)
		},
		error: function (err) {
			if (error_callback != null)
				error_callback(err);
		}
	});
}
function AddMensaje(status, okenable=false,callback=null)
{
	if(status.Code != 0 || (status.Code == 0 && okenable))
	{
		let tag = document.createElement("div");
		tag.className = "mensaje_item ";
		if(status.Code == 500)
			tag.className += "mensaje_error";
		else if (okenable &&  status.Code == 0)
			tag.className += "mensaje_ok";
		else
			tag.className += "mensaje_advetencia";

		let text = document.createElement("h4");
	  	text.innerHTML = status.Description;
	   	tag.appendChild(text);
	   	
		let message_container = document.getElementById("message_container");
		message_container.appendChild(tag);
		
		setTimeout(() =>
		{
			tag.style.marginLeft = "10px";
			setTimeout(() =>
			{
				message_container.removeChild(tag);
				/*
				if(callback != null)
					callback();
				*/
				
			},3000);
		}, 50);
	}
}