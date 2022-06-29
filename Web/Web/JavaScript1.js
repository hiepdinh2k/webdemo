   
    $(document).ready(function () {
    $("#cmdlogin").click(function () {

        dialog_login = $.confirm({
            title: 'Đăng Nhập',
            content: '' +
            '<form action="" class="formName">' +
           '<table><div class="form-group">' +
            '<label>Nhập tài khoản:</label>' +
            '<input type="text" placeholder="Username" class="msv form-control" required />' +
            '</div>' +
            '<div class="form-group">' +
            '<label>Nhập mật khẩu:</label>' +
            '<input type="password" placeholder="password" class="pw form-control" required />' +
            '</div>' +
             '<td><label>Captcha:</label></td>' +
            '<td> <img src="capcha.ashx"/> ' +
            '<td><input type="text" placeholder="Nhập captcha" class="cap form-control" required /></td>' +
            '</tr>' +
            '</table></form>',
            buttons: {
                formSubmit: {
                    text: 'Đăng nhập',
                    btnClass: 'btn-blue',

                    action: function () {
                        var msv = this.$content.find('.msv').val();
                        if (!msv) {
                            $.alert('chưa nhập tài khoản');
                            return false;
                        }
                        var pw = this.$content.find('.pw').val();
                        if (!pw) {
                            $.alert('chưa nhập mật khẩu');
                            return false;
                        }
                        var cap = this.$content.find('.cap').val();
                            if (!cap) {
                                $.alert('Phải nhập captcha !');
                                return false;
                            
                        }
                        $.post("xl.aspx",
                             {
                                 action: "login",
                                 msv: msv,
                                 pw: pw,
                                 cap: cap
                             },
                             function (data,thongbao) {
                                 //data chính là chuỗi json gửi về
                                 var json = JSON.parse(data);
                                 if (json.ok) {
                                     if (thongbao) {
                                         dialog_login.close();
                                         $.alert({
                                             title: 'Đăng nhập thành công',
                                             autoClose: 'ok|10',
                                         });
                                     }
                                     $('#login').hide();
                                     $('#ID2').show();
                                     $('#welcome').html('Xin chào!  ' + json.sv.msv);
                                    // thisDialog.close();
                                 } else {
                                     if (thongbao)
                                         $.alert({
                                             title: 'Báo lỗi',
                                             content: json.msg,
                                             autoClose: 'ok|3000',
                                         });
                                 }
                             });
                    }
                },
                cancel: function () {
                    //close
                },
            },
            onContentReady: function () {
                // bind to events
                var jc = this;
                this.$content.find('form').on('submit', function (e) {
                    // if the user submits the form by pressing enter in the field.
                    e.preventDefault();
                    jc.$$formSubmit.trigger('click'); // reference the button and click it
                });
            }
        });
    })
    function checklogin() {
        $.post("xl.aspx",
        {
            action: 'check_login'
        },
        function (data, thongbao) {
            var json = JSON.parse(data);
            if (json.ok) {

                $('#login').hide();
                $('#ID2').show();
                $('#welcome').html('Xin chào!  ' + json.msv);
            }
        });
    }
    function logout() {
      
             $('#login').show();
             $('#ID2').hide();

    }

    function doimk() {
        dialog_changepass = $.confirm({
            title: 'Đổi mật khẩu',
            content: '' +
            '<form action="" class="formSV">' +
            '<table><tr>' +
            '<td><label>Mật khẩu cũ:</label></td>' +
            '<td><input type="password" placeholder="Nhập mật khẩu cũ" class="pwc form-control" required /></td>' +
            '</tr>' +
             '<tr>' +
            '<td><label>Mật khẩu mới:</label></td>' +
            '<td><input type="password" placeholder="Nhập mật khẩu mới" class="pw form-control" required /></td>' +
            '</tr>' +
            '</table></form>',
            buttons: {
                formSubmit: {
                    text: 'Đổi mật khẩu',
                    btnClass: 'btn-blue',
                    action: function () {

                        var pwc = this.$content.find('.pwc').val();
                        if (!pwc) {
                            $.alert('nhập mật khẩu cũ!');
                            return false;
                        }
                        var pw = this.$content.find('.pw').val();
                        if (!pw) {
                            $.alert('nhập mật khẩu mới!');
                            return false;
                        }

                        $.post("xl.aspx",
                        {
                            action: 'pwm',
                            pwc : pwc,
                            pw : pw

                        },
                        function (data, thongbao) {
                            var json = JSON.parse(data);
                            if (json.ok) {

                                if (thongbao) {
                                    dialog_changepass.close();
                                    $.alert({
                                        title: 'Đổi thành công',
                                        autoClose: 'ok|3000',
                                    });

                                }

                            } else {

                                if (thongbao)
                                    $.alert({
                                        title: 'Báo lỗi',
                                        content: json.msg,
                                        autoClose: 'ok|3000',
                                    });
                            }

                        });
                        return false;
                    }
                },
                cancel: function () {

                },
            },
            onContentReady: function () {

                var jc = this;
                this.$content.find('form').on('submit', function (e) {

                    jc.$$formSubmit.trigger('click');
                });
            }
        });
    }
    function lichsu() {
        $.post("xl.aspx",
        {
            action: "lichsu"
        },
        function (data) {
            var json = JSON.parse(data);
            if (json.ok) {
                var mymsg = 'Lịch sử đăng nhập'
                var html = '<p>' + mymsg + '</p><table class="table table-myhover">';
                html += '<thead><tr>' +
                        '<th>STT</th>' +
                        '<th>Lịch sử</th>' +
                        '</tr></thead><tbody>';
                for (var i = 0; i < json.lichsu.length; i++) {
                    var ls =json.lichsu[i];
                    html += '<tr>' +
                          '<td>' + (1 + i) + '</td>' +
                          '<td>' + ls.noidung + '</td>' +
                          '</tr>';
                }
                html += '</tbody></table>';
                $('#LS').html(html);
            }
        });

    }
    $("#cmddoimatkhau").click(function () {
        doimk();
    });
    $("#cmdlogout").click(function () {
        logout();
    });
    $("#cmdlichsu").click(function () {
        lichsu();
    });
    checklogin();
});