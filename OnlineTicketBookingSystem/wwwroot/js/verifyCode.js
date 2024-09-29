$(document).ready(function () {
    initializeModal(); // Khởi tạo modal
    initializeOtpInputs(); // Khởi tạo ô nhập OTP
    initializeOtpSubmission(); // Khởi tạo việc xác nhận OTP
    initializeResendOtp(); // Khởi tạo việc gửi lại mã OTP
});

function initializeModal() {

    //$('#accountModal').modal('show');

    $("#nextToOtpModal").click(function () {
        $('#accountModal').modal('hide');
        $('#otpModal').modal('show');
        resetCountdown();
        startCountdown();
    });
}

function initializeOtpInputs() {
    $(".otp-input").on("input", function () {
        toggleInputState($(this));
        if ($(this).val().length >= $(this).attr("maxlength")) {
            $(this).next(".otp-input").focus();
        }
    });

    $(".otp-input").on("click", function () {
        $(this).focus();
    });

    $(".otp-input").on("keydown", function (e) {
        handleKeyDown(e, $(this));
    });
}

function toggleInputState(input) {
    if (input.val()) {
        input.removeClass("empty").addClass("filled");
    } else {
        input.removeClass("filled").addClass("empty");
    }
}

function handleKeyDown(e, input) {
    if (e.key >= 0 && e.key <= 9) {
        input.val(e.key);
        toggleInputState(input);
        input.next(".otp-input").focus();
        e.preventDefault();
    } else if (e.key === "Backspace") {
        e.preventDefault();
        if (input.val() === '') {
            var prevInput = input.prev(".otp-input");
            if (prevInput.length) {
                prevInput.focus();
            }
        } else {
            input.val('');
            toggleInputState(input);
        }
    }
}

function initializeOtpSubmission() {
    $("#submitOtp").click(function () {
        var otp = getOtp();
        const validOtp = "123456"; // Thay đổi mã xác thực cứng nếu cần

        if (otp.length === 6) {
            validateOtp(otp, validOtp);
        } else {
            showOtpFeedback("Vui lòng nhập đầy đủ mã xác thực!", "red");
        }
    });
}

function getOtp() {
    var otp = "";
    $(".otp-input").each(function () {
        otp += $(this).val();
    });
    return otp;
}

function validateOtp(otp, validOtp) {
    if (otp === validOtp) {
        showOtpFeedback("Mã xác thực thành công!", "green");
        clearInterval(countdownInterval);
        $("#resendMessage").hide();
    } else {
        showOtpFeedback("Mã xác thực không tồn tại!", "red");
    }
}

function showOtpFeedback(message, color) {
    $("#otpFeedback")
        .css("color", color)
        .text(message)
        .show();
}

function initializeResendOtp() {
    $("#resendOtp").click(function () {
        resetCountdown();
        startCountdown();
        $("#resendMessage").hide();
        resetOtpInputs();
        enableOtpInputs();
    });
}

function resetOtpInputs() {
    $(".otp-input").val('').removeClass("filled").addClass("empty");
    $("#otpFeedback").hide();
}

function enableOtpInputs() {
    $(".otp-input").prop("disabled", false);
    $("#submitOtp").prop("disabled", false);
}

let countdownTime;
let countdownInterval;

function resetCountdown() {
    countdownTime = 5;
    $("#countdown").text(countdownTime + "s");
    $("#resendMessage").hide();
}

function startCountdown() {
    countdownInterval = setInterval(function () {
        countdownTime--;
        $("#countdown").text(countdownTime + "s");

        if (countdownTime <= 0) {
            clearInterval(countdownInterval);
            $(".otp-input").prop("disabled", true);
            showOtpFeedback("Mã xác thực đã hết hạn!", "red");
            $("#resendMessage").show();
            $("#submitOtp").prop("disabled", true);
        }
    }, 1000);
}
