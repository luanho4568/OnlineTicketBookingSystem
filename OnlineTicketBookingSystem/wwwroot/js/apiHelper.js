var apiMain = {
    apiWithBearer: function (url, method, data, successCallback, errorCallback) {
        const token = localStorage.getItem('authToken');
        $.ajax({
            url: url,
            type: method,
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            headers: {
                'Authorization': 'Bearer ' + token
            },
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (error) {
                if (typeof errorCallback === "function") {
                    errorCallback(error);
                }
            }
        });
    },
    api: function (url, method, data, successCallback, errorCallback) {
        $.ajax({
            url: url,
            type: method,
            data: JSON.stringify(data),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (typeof successCallback === "function") {
                    successCallback(response);
                }
            },
            error: function (error) {
                if (typeof errorCallback === "function") {
                    errorCallback(error);
                }
            }
        });
    }

}

export default apiMain;
