function SearchProduct() {
    var filter = $('#searchProduct').val();

    if (filter == "") {
        alert("Ingrese busqueda del producto");
    } else {
        var serviceURL = '/Home/getProduct';

        $.ajax({
            type: 'POST',
            url: serviceURL,
            data: { filter: filter },
            error: function () {
            },
            success: function (result) {
                SetProduct(result);
            }
        });
    }
}

var pId = 0;
var catId = 0;
var quantity = 0;
var price = 0;

function SetProduct(response) {
    $('#formAddProduct').attr('style', 'block');
    $('#btnAddProduct').attr('style', 'block');
    $('#tableAllProduct').attr('style', 'none');

    pId = response["pId"];
    catId = response["catId"];
    quantity = response["quantity"];
    price = response["pPrice"];

    $("#pName").val(response["pName"]);
    $("#customer").val("Cliente Rapido");
    $("#catName").val(response["catName"]);
    $("#pPrice").val(response["pPrice"]);
}

var totSale = 0;
var lstProducts = [];

function AddProduct() {
    var exists = 0;

    var Product = {};
    Product.pId = pId;
    Product.catId = catId;
    Product.price = parseInt($('#pPrice').val()); 
    Product.quantity = parseInt($('#quantity').val()); 

    if (Product.quantity == "") {
        alert("Ingrese cantidad");
    } else {
        if (parseInt(Product.quantity) > parseInt(quantity)) {
            alert("El producto no puede tener una cantidad mayor a la disponible");
        }
        else {
            if (lstProducts.length == 0) {
                lstProducts.push(Product);
                Product = {};
                calculateTotSale();
            }
            else {
                if (lstProducts[0].catId != catId) {
                    alert("No se permite agregar productos de otra categoría");
                }
                else {
                    for (k in lstProducts) {
                        if (lstProducts[k].pId == pId) {
                            lstProducts[k].quantity = Product.quantity;
                            calculateTotSale();
                            exists = 1;
                        }
                    }
                    if (exists == 0) {
                        lstProducts.push(Product);
                        Product = {};
                        calculateTotSale();
                    }
                }
            }
        }
    }

    //LIMPIAR VARIABLES
    pId = 0;
    catId = 0;
    quantity = 0;
    price = 0;
    $("#searchProduct").val("");
    $("#pName").val("");
    $("#customer").val("");
    $("#catName").val("");
    $("#pPrice").val("");
    $("#quantity").val("");
}

function calculateTotSale() {
    $('#btnFinish').attr('style', 'block');

    totSale = 0;
    for (var i = 0; i < lstProducts.length; i++) {
        totSale += parseInt(lstProducts[i].price) * parseInt(lstProducts[i].quantity);
    }
    $("#totSale").val(totSale);

    console.log(lstProducts);
    console.log(totSale);
}

function FinishSale() {
    var serviceURL = '/Home/FinishSale';

    $.ajax({
        type: 'POST',
        url: serviceURL,
        data: { json: JSON.stringify(lstProducts) },
        error: function () {
        },
        success: function (result) {
            alert("El pedido se finalizó con exito");
            clearData();
        }
    });
}


function clearData() {
    pId = 0;
    catId = 0;
    quantity = 0;
    price = 0;
    totSale = 0;
    lstProducts = [];

    $("#searchProduct").val("");
    $("#pName").val("");
    $("#customer").val("");
    $("#catName").val("");
    $("#pPrice").val("");
    $("#quantity").val("");
    $("#totSale").val("");

    $('#formAddProduct').attr('style', 'none');
    $('#btnAddProduct').attr('style', 'none');
    $('#btnFinish').attr('style', 'none');
    $('#tableAllProduct').attr('style', 'block');
}
