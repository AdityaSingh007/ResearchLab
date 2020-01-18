var AnalyticRequest = function () {
    var uploadCIMFilePath = '/Home/UploadCIMFile',
        sendAcknowledgementEmailPath = '/Home/SendAcknowledgementEmail',
        saveDASAnalyticRequestPath = '/Home/InsertUpdateAnalytics',
        IsDasRequestNameAlreadyTakenPath = '/Home/IsDasRequestNameAlreadyTaken',
        TriggerDasEmailWorkFlow = function () {

            AnalyticRequest.IsDasRequestNameAlreadyTaken().then(function (response, textStatus, jqXHR) {
                if (response === "true") {
                    AnalyticRequest.SaveDASAnalyticRequest().then(function (response, textStatus, jqXHR) {

                        if (response === "Success") {
                            toastr.success("Request saved successfully");
                            AnalyticRequest.UploadCIMFile().then(function (response, textStatus, jqXHR) {
                                if (response === "error")
                                    toastr.error("Error uploading file");
                                else
                                    toastr.success("File uploaded successfully");
                                AnalyticRequest.SendAcknowledgementEmail().then(function (response, textStatus, jqXHR) {
                                    if (response === "Success")
                                        toastr.success("Email sent successfully");
                                    else
                                        toastr.error("Error processing request");

                                });
                            })
                        }
                        else
                            toastr.error("Error processing request");
                    })
                }
                else {
                    toastr.error("Request name has already been taken");
                }

            });
        },
        IsDasRequestNameAlreadyTaken = function (analyticsId) {

            return $.ajax({
                url: IsDasRequestNameAlreadyTakenPath,
                type: 'GET',
                async: true,
                data: { analyticsId: parseInt("111") },
                contentType: 'application/json; charset=utf-8'
            });
        },
        uploadCIMFile = function () {

            return $.ajax({
                url: uploadCIMFilePath,
                type: 'POST',
                async: true,
                contentType: 'application/json; charset=utf-8'
            });

        },
        sendAcknowledgementEmail = function () {
            return $.ajax({
                url: sendAcknowledgementEmailPath,
                type: 'POST',
                async: true,
                contentType: 'application/json; charset=utf-8'
            });
        },
        saveDASAnalyticRequest = function () {
            return $.ajax({
                url: saveDASAnalyticRequestPath,
                type: 'POST',
                async: true,
                contentType: 'application/json; charset=utf-8'
            });
        },

        init = function () {
            $('#btnSendEmail').click(function () {
                AnalyticRequest.TriggerDasEmailWorkFlow();
            })
        }


    return {
        UploadCIMFile: uploadCIMFile,
        SendAcknowledgementEmail: sendAcknowledgementEmail,
        SaveDASAnalyticRequest: saveDASAnalyticRequest,
        IsDasRequestNameAlreadyTaken: IsDasRequestNameAlreadyTaken,
        TriggerDasEmailWorkFlow: TriggerDasEmailWorkFlow,
        Init: init
    }
}();