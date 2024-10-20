import apiMain from '/js/apiHelper.js';

var countdownTimer;
var timeLeft = 30;

$(document).ready(function () {
    $('#loginForm').on('submit', function (e) {
        e.preventDefault();
        const user = {
            Email: $('#email').val(),
            Password: $('#password').val()
        };

        // Sử dụng apiMain.api cho Login (không cần Bearer token)
        apiMain.api('/api/v1/Auth/Login', 'POST', user, function (response) {
            console.log(response);
            if (response.code === 403) {
                initializeModal(user);
            } else if (response.code === 200) {
                localStorage.setItem('authToken', response.token);
                localStorage.setItem('user', JSON.stringify({ fullName: response.fullName, group: response.group, avatar: response.avatar }));
                var token = localStorage.getItem('authToken');
                if (token) {
                    if (response.group == 1) {
                        window.location.href = '/Admin/Home';
                    }
                    //else if (response.group == 2) {
                    //    window.location.href = '/Driver/Home';
                    //} else if (response.group == 3) {
                    //    window.location.href = '/Customer/Home';
                    //}
                }
            }
        }, function (errormessage) {
            console.log(errormessage.responseText);
        });
    });

    $('#submitOtp').click(function () {
        const otpCode = $('#otpInput').val().toUpperCase();
        const userEmail = $('#email').val();
        const userPassword = $('#password').val();

        // Sử dụng apiMain.apiWithBearer cho VerifyCode (cần Bearer token)
        apiMain.apiWithBearer('/api/v1/Auth/VerifyCode', 'POST', { Email: userEmail, Password: userPassword, CodeId: otpCode }, function (response) {
            console.log(response);
            if (response.code === 200) {
                $('#otpModal').modal('hide');
                Swal.fire({
                    position: "center",
                    icon: "success",
                    title: response.message,
                    showConfirmButton: false,
                    timer: 1500
                });
            } else if (response.code === 404) {
                $('#otpFeedback').text(response.message).css('color', 'red').show();
            }
        }, function (errormessage) {
            console.log(errormessage.responseText);
        });
    });

    // Gửi lại mã OTP
    $('#resendOtp').click(function () {
        const userEmail = $('#email').val();
        const userPassword = $('#password').val();

        // Sử dụng apiMain.api cho CheckActivation (không cần Bearer token)
        apiMain.api('/api/v1/Auth/CheckActivation', 'POST', { Email: userEmail, Password: userPassword }, function (response) {
            if (response.code === 200) {
                $('#otpFeedback').hide();
                resetCountdown();
                startCountdown();
            }
        }, function (errormessage) {
            console.log(errormessage.responseText);
        });
    });
});

function initializeModal(user) {
    $('#accountModal').modal('show');
    $("#nextToOtpModal").click(function () {

        // Sử dụng apiMain.api cho CheckActivation (không cần Bearer token)
        apiMain.api('/api/v1/Auth/CheckActivation', 'POST', { Email: user.Email, Password: user.Password }, function (response) {
            if (response.code === 200) {
                $('#accountModal').modal('hide');
                $('#otpModal').modal('show');
                resetCountdown();
                startCountdown();
            }
        }, function (errormessage) {
            console.log(errormessage.responseText);
        });
    });
}

// Reset thời gian đếm ngược
function resetCountdown() {
    timeLeft = 120;
    $('#countdown').text(timeLeft + "s");
    $('#resendMessage').hide();
    $('#otpFeedback').hide();
}

// Bắt đầu đếm ngược thời gian
function startCountdown() {
    countdownTimer = setInterval(function () {
        timeLeft--;
        $('#countdown').text(timeLeft + "s");

        if (timeLeft <= 0) {
            clearInterval(countdownTimer);
            $('#countdown').text("Mã xác thực đã hết hạn!");
            $('#resendMessage').show(); // Hiển thị nút gửi lại mã OTP
        }
    }, 1000);
}
