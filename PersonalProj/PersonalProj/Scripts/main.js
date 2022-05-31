/**
main javascipr function here

using jquery and ajax
*/

function ShowLoginModal(){
    $("#LoginModal").modal('show');
}

function Login(url){
        var userid = $("#Login_UserID").val();
        var pass = $("#Login_Password").val();
        $.ajax({
          url: url,
          type: 'POST',
          data: {
              UserName : userid,
              Password : pass
          },
          success: function (data) {
                if(data.success){
                    $("#LoginModal").modal('hide');
                    location.reload();
                }else{
                    $("#Login_Error").text(data.responseText);
                    console.log(data.responseText);
                    }
              
          },
          error: function (error) {
              
              console.log("error is " + error);
          }
        });
}
function Logout(url){
    $.ajax({
          url: url,
          type: 'GET',
          success: function (data) {
              location.reload();
          },
          error: function (error) {
              console.log("error is " + error);
          }
        });
}