var timeLeft;
var countdownTimer;
$(document).ready(function () {
    $('#loginForm').on('submit', function (e) {
        e.preventDefault();
        const user = {
            Email: $('#email').val(),
            Password: $('#password').val()
        };
        $.ajax({
            url: '/api/v1/Auth/Login',
            type: 'POST',
            data: JSON.stringify(user),
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.code === 403) {
                    initializeModal(user);
                } else if (response.code === 200) {
                    Swal.fire({
                        icon: "success",
                        title: response.message,
                        showConfirmButton: false,
                        timer: 1500
                    }).then(() => {
                        localStorage.setItem('authToken', response.token);
                        localStorage.setItem('user', JSON.stringify({ group: response.group, avatar: response.avatar }));
                        var token = localStorage.getItem('authToken');
                        if (token) {
                            if (response.group == 1) {
                                window.location.href = '/Admin/Home';
                            }
                            else if (response.group == 2) {
                               
                                window.location.href = '/Driver/Home';
                            }
                            else if (response.group == 3) {
                                console.log(token)
                                console.log(response.group)
                                window.location.href = '/';
                            }
                        }
                    })
                }
            },
            error: function (errormessage) {
                Swal.fire({
                    icon: "error",
                    title: "Đăng nhập thất bại",
                    text: JSON.parse(errormessage.responseText).message,
                });
                console.log(errormessage.responseText);
            }
        });

        $('#submitOtp').click(function () {
            const otpCode = $('#otpInput').val().toUpperCase();
            const userEmail = $('#email').val();
            const userPassword = $('#password').val();
            $.ajax({
                url: '/api/v1/Auth/VerifyCode',
                type: 'POST',
                data: JSON.stringify({ Email: userEmail, Password: userPassword, CodeId: otpCode }),
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.code === 200) {
                        $('#otpModal').modal('hide');
                        Swal.fire({
                            position: "center",
                            icon: "success",
                            title: response.message,
                            showConfirmButton: false,
                            timer: 1500
                        });
                    }
                },
                error: function (errormessage) {
                    $('#otpFeedback').text(JSON.parse(errormessage.responseText).message).css('color', 'red').show();
                }
            });
        });
        // Gửi lại mã OTP
        $('#resendOtp').click(function () {
            const userEmail = $('#email').val();
            const userPassword = $('#password').val();
            $.ajax({
                url: '/api/v1/Auth/CheckActivation',
                type: 'POST',
                data: JSON.stringify({ Email: userEmail, Password: userPassword }),
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
            $(this).prop("disabled", true);
            $.ajax({
                url: '/api/v1/Auth/CheckActivation',
                type: 'POST',
                data: JSON.stringify({ Email: user.Email, Password: user.Password }),
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
                },
                complete: function () {
                    $(this).prop("disabled", false); // Kích hoạt lại nút sau khi ajax hoàn tất
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

        // Dừng bộ đếm nếu có
        if (countdownTimer) {
            clearInterval(countdownTimer);
        }
    }

    // Bắt đầu đếm ngược thời gian
    function startCountdown() {
        // Dừng bộ đếm trước khi khởi động lại
        if (countdownTimer) {
            clearInterval(countdownTimer);
        }

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
})
