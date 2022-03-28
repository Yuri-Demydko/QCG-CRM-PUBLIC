function sendCode(data) {
    console.log(data);
    $.ajax({
        type: 'POST',
        url: '/Account/SendVerifyEmailForRegistration',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(data),
        success: function (result) {
            console.log('Successfully received Data');
            console.log(result);
            $('#confirmCode').show();
        },
        error: function (result) {
            console.log('Failed to receive the Data');
            console.log(result);
        }
    })
}

let email = $("#SendCodeEmail").val();
document.getElementById("SendCodeButton").addEventListener("click",function (){sendCode(email)});
let hideForm = $("#hideForm").val();
if(hideForm==="true")
{
    $('#confirmCode').show();
}