var location = {
    loadDistricts: function (provinceCode) {
        $.ajax({
            url: '/api/v1/Location/GetDistrictsByProvince',
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
    },

    loadWards: function (districtCode) {
        $.ajax({
            url: '/api/v1/Location/GetWardsByDistrict',
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
}

export default location