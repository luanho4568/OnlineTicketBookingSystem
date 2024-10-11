var countdownTimer;
var timeLeft = 30;

$(document).ready(function () {
    $('#loginForm').on('submit', function (e) {
        e.preventDefault();
        const user = {
            Email: $('#email').val(),
            Password: $('#password').val()
        };
        $.ajax({
            url: '/v1/AuthAPI/Login',
            type: 'POST',
            data: JSON.stringify(user),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (response) {
                console.log(response);
                if (response.code === 403) {
                    initializeModal(user);
                } else if (response.code === 200) {
                    window.location.href = '/Admin/Home';
                }
            },
            error: function (errormessage) {
                console.log(errormessage.responseText);
            }
        });
    });

    $('#submitOtp').click(function () {
        const otpCode = $('#otpInput').val().toUpperCase();
        const userEmail = $('#email').val();

        $.ajax({
            url: '/v1/AuthAPI/VerifyCode',
            type: 'POST',
            data: JSON.stringify({ Email: userEmail, CodeId: otpCode }),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (response) {
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
            },
            error: function (errormessage) {
                console.log(errormessage.responseText);
            }
        });
    });


    // Gửi lại mã OTP
    $('#resendOtp').click(function () {
        const userEmail = $('#email').val();

        $.ajax({
            url: '/v1/AuthAPI/CheckActivation',
            type: 'POST',
            data: JSON.stringify({ Email: userEmail }),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.code === 200) {
                    $('#otpFeedback').hide();
                    resetCountdown();
                    startCountdown();
                }
            },
            error: function (errormessage) {
                console.log(errormessage.responseText);
            }
        });
    });
});

function initializeModal(user) {
    $('#accountModal').modal('show');
    $("#nextToOtpModal").click(function () {
        $.ajax({
            url: '/v1/AuthAPI/CheckActivation',
            type: 'POST',
            data: JSON.stringify({ Email: user.Email }),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.code === 200) {
                    $('#accountModal').modal('hide');
                    $('#otpModal').modal('show');
                    resetCountdown();
                    startCountdown();
                }
            },
            error: function (errormessage) {
                console.log(errormessage.responseText);
            }
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
