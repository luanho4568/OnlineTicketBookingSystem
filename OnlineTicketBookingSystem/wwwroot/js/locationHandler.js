function loadDistricts(provinceCode) {
    $.ajax({
        url: '/Api/Location/GetDistrictsByProvince', // Sửa URL thành đúng API route
        type: 'GET',
        data: { provinceCode: provinceCode },
        success: function (data) {
            $('#districtDropdown').empty(); // Xoá các option hiện tại
            $('#districtDropdown').append('<option disabled selected>-- Chọn huyện --</option>');
            $.each(data, function (index, option) {
                $('#districtDropdown').append('<option value="' + option.value + '">' + option.text + '</option>');
            });
        },
        error: function () {
            console.error("Đã xảy ra lỗi khi lấy danh sách huyện.");
        }
    });
}

function loadWards(districtCode) {
    $.ajax({
        url: '/Api/Location/GetWardsByDistrict', // Sửa URL thành đúng API route
        type: 'GET',
        data: { districtCode: districtCode },
        success: function (data) {
            $('#wardDropdown').empty(); // Xoá các option hiện tại
            $('#wardDropdown').append('<option disabled selected>-- Chọn phường/xã --</option>');
            $.each(data, function (index, option) {
                $('#wardDropdown').append('<option value="' + option.value + '">' + option.text + '</option>');
            });
        },
        error: function () {
            console.error("Đã xảy ra lỗi khi lấy danh sách phường.");
        }
    });
}
