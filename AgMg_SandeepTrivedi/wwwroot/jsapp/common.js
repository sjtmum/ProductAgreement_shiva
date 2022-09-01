function loadProduct(val) {
    let dropdown = $('#ProductId');
    dropdown.empty();
    dropdown.append('<option selected="true" disables>Choose Product</option>');
    dropdown.prop('selectedIndex', 0);
    $.ajax({
        type: 'Get',
        //url: 'GetProductList',
        url: '/Agreements/GetProductList',
        //url: '@(Url.Action("Agreements", "GetProductList"))',
        dataType: 'json',
        data: 'prodGroupId=' + val,
        success: function (prodList) {
            $.each(prodList, function (i, item) {
                dropdown.append("<option value='" + item.value + "'>" + item.text + "</option>")
            });
        },
        error: function (ex) {
            alert('Failed to retrieve Product List :' + ex);
        }
    }); //ajax end
}