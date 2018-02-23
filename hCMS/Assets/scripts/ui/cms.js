$(function () {
    cms.init();
    $(document).ajaxError(function (e, xhr) {
        if (xhr.status === 403) {
            var response = $.parseJSON(xhr.responseText);
        }
    });
});
var cmsConfigs = {
    rootPath: '/'
};

var cms = {
    init: function () {
        this.events();
    },
    events: function () {
        $('.logout').off('click').on('click', function (e) {
            e.preventDefault();
            $().lawsDialog({
                messages: ['Bạn muốn đăng xuất khỏi hệ thống?']
                , buttons: [
                    {
                        text: 'Hủy',
                        class: 'btn-thongbao1',
                        click: function () {
                            $(this).dialog('close');
                        }
                    },
                    {
                        text: 'Đồng ý',
                        click: function () {
                            $(this).dialog('close');
                            window.location.href = cms.virtualPath('/dang-xuat-tai-khoan');
                        }
                    }
                ]
            });
        });

        $('.delete-action').off('click').on('click', function (e) {
            var actionId = $(this).data('id');
            e.preventDefault();
            $().lawsDialog({
                messages: ['Xác nhận xóa chức năng?']
                , buttons: [
                    {
                        text: 'Hủy',
                        class: 'btn-thongbao2',
                        click: function () {
                            $(this).dialog('close');
                        }
                    },
                    {
                        text: 'Đồng ý',
                        class: 'btn-thongbao1',
                        click: function () {
                            $(this).dialog('close');
                            $.lawsAjax({
                                url: cms.virtualPath('/Ajax/ActionDeleteById'),
                                data: { actionId: actionId },
                                success: function (resp) {
                                    if (resp.Completed) {
                                        if (resp.Message !== null && resp.Message.length > 0) {
                                            $().lawsDialog({
                                                dialogClass: 'lawsVnDialogTitle',
                                                messages: [resp.Message],
                                                onClose: function() {
                                                    if (resp.ReturnUrl !== null && resp.ReturnUrl.length > 0)
                                                        setTimeout(function () {
                                                                window.location.href = resp.ReturnUrl;
                                                                location.reload();
                                                            },
                                                            100);
                                                    else window.location.href = cmsConfigs.rootPath;
                                                }
                                            });
                                        }
                                    }
                                }
                            });
                        }
                    }
                ]
            });
        });

        $('.delete-actions').off('click').on('click',
            function (event) {
                event.preventDefault();
                var actionsId = $('.chk-action input[type=checkbox]:checked').map(function (_, el) {
                    return $(el).val();
                }).get();
                if (!actionsId.length) {
                    $().lawsDialog({
                        dialogClass: 'lawsVnDialogTitle',
                        messages: ['Bạn chưa chọn chức năng cần xóa.']
                    });
                    return;
                }
                $().lawsDialog({
                    dialogClass: 'lawsVnDialogTitle',
                    messages: ['Xác nhận xóa chức năng đã chọn ?'],
                    buttons: [
                        {
                            text: 'Đóng',
                            class: 'btn-thongbao2',
                            click: function () {
                                $(this).dialog('close');
                            }
                        },
                        {
                            text: 'ĐỒNG Ý',
                            class: 'btn-thongbao1',
                            click: function () {
                                $(this).dialog('close');
                                $.lawsAjax({
                                    url: cms.virtualPath('/Ajax/ActionDelete'),
                                    type: 'Post',
                                    traditional: true,
                                    data: { actionsId: actionsId },
                                    success: function (resp) {
                                        if (resp.Message !== void 0 && resp.Message.length > 0) {
                                            $().lawsDialog({
                                                messages: [resp.Message],
                                                showIcon: false,
                                                onClose: function () {
                                                    if (resp.Completed && resp.ReturnUrl !== void 0 && resp.ReturnUrl.length > 0) {
                                                        window.location.href = resp.ReturnUrl;
                                                    }
                                                }
                                            });
                                        }
                                    }
                                });
                            }
                        }
                    ]
                });
            });

        $('.delete-role').off('click').on('click', function (e) {
            var roleId = $(this).data('id');
            e.preventDefault();
            $().lawsDialog({
                messages: ['Xác nhận xóa quyền?']
                , buttons: [
                    {
                        text: 'Hủy',
                        class: 'btn-thongbao2',
                        click: function () {
                            $(this).dialog('close');
                        }
                    },
                    {
                        text: 'Đồng ý',
                        class: 'btn-thongbao1',
                        click: function () {
                            $(this).dialog('close');
                            $.lawsAjax({
                                url: cms.virtualPath('/Ajax/RoleDeleteById'),
                                data: { roleId: roleId },
                                success: function (resp) {
                                    if (resp.Completed) {
                                        if (resp.Message !== null && resp.Message.length > 0) {
                                            $().lawsDialog({
                                                dialogClass: 'lawsVnDialogTitle',
                                                messages: [resp.Message],
                                                onClose: function () {
                                                    if (resp.ReturnUrl !== null && resp.ReturnUrl.length > 0)
                                                        setTimeout(function () {
                                                            window.location.href = resp.ReturnUrl;
                                                            location.reload();
                                                        },
                                                            100);
                                                    else window.location.href = cmsConfigs.rootPath;
                                                }
                                            });
                                        }
                                    }
                                }
                            });
                        }
                    }
                ]
            });
        });

        $('.delete-roles').off('click').on('click',
            function (event) {
                event.preventDefault();
                var actionsId = $('.chk-action input[type=checkbox]:checked').map(function (_, el) {
                    return $(el).val();
                }).get();
                if (!actionsId.length) {
                    $().lawsDialog({
                        dialogClass: 'lawsVnDialogTitle',
                        messages: ['Bạn chưa chọn quyền cần xóa.']
                    });
                    return;
                }
                $().lawsDialog({
                    dialogClass: 'lawsVnDialogTitle',
                    messages: ['Xác nhận xóa quyền đã chọn ?'],
                    buttons: [
                        {
                            text: 'Đóng',
                            class: 'btn-thongbao2',
                            click: function () {
                                $(this).dialog('close');
                            }
                        },
                        {
                            text: 'ĐỒNG Ý',
                            class: 'btn-thongbao1',
                            click: function () {
                                $(this).dialog('close');
                                $.lawsAjax({
                                    url: cms.virtualPath('/Ajax/RoleDelete'),
                                    type: 'Post',
                                    traditional: true,
                                    data: { actionsId: actionsId },
                                    success: function (resp) {
                                        if (resp.Message !== void 0 && resp.Message.length > 0) {
                                            $().lawsDialog({
                                                messages: [resp.Message],
                                                showIcon: false,
                                                onClose: function () {
                                                    if (resp.Completed && resp.ReturnUrl !== void 0 && resp.ReturnUrl.length > 0) {
                                                        window.location.href = resp.ReturnUrl;
                                                    }
                                                }
                                            });
                                        }
                                    }
                                });
                            }
                        }
                    ]
                });
            });

        $('.delete-user').off('click').on('click', function (e) {
            var userId = $(this).data('id');
            e.preventDefault();
            $().lawsDialog({
                messages: ['Xác nhận xóa user?']
                , buttons: [
                    {
                        text: 'Hủy',
                        class: 'btn-thongbao2',
                        click: function () {
                            $(this).dialog('close');
                        }
                    },
                    {
                        text: 'Đồng ý',
                        class: 'btn-thongbao1',
                        click: function () {
                            $(this).dialog('close');
                            $.lawsAjax({
                                url: cms.virtualPath('/Ajax/UserDeleteById'),
                                data: { userId: userId },
                                success: function (resp) {
                                    if (resp.Completed) {
                                        if (resp.Message !== null && resp.Message.length > 0) {
                                            $().lawsDialog({
                                                dialogClass: 'lawsVnDialogTitle',
                                                messages: [resp.Message],
                                                onClose: function () {
                                                    if (resp.ReturnUrl !== null && resp.ReturnUrl.length > 0)
                                                        setTimeout(function () {
                                                            window.location.href = resp.ReturnUrl;
                                                            location.reload();
                                                        },
                                                            100);
                                                    else window.location.href = cmsConfigs.rootPath;
                                                }
                                            });
                                        }
                                    }
                                }
                            });
                        }
                    }
                ]
            });
        });

        $('.delete-users').off('click').on('click',
            function (event) {
                event.preventDefault();
                var usersId = $('.chk-action input[type=checkbox]:checked').map(function (_, el) {
                    return $(el).val();
                }).get();
                if (!usersId.length) {
                    $().lawsDialog({
                        dialogClass: 'lawsVnDialogTitle',
                        messages: ['Bạn chưa chọn user cần xóa.']
                    });
                    return;
                }
                $().lawsDialog({
                    dialogClass: 'lawsVnDialogTitle',
                    messages: ['Xác nhận xóa user đã chọn ?'],
                    buttons: [
                        {
                            text: 'Đóng',
                            class: 'btn-thongbao2',
                            click: function () {
                                $(this).dialog('close');
                            }
                        },
                        {
                            text: 'ĐỒNG Ý',
                            class: 'btn-thongbao1',
                            click: function () {
                                $(this).dialog('close');
                                $.lawsAjax({
                                    url: cms.virtualPath('/Ajax/UserDelete'),
                                    type: 'Post',
                                    traditional: true,
                                    data: { usersId: usersId },
                                    success: function (resp) {
                                        if (resp.Message !== void 0 && resp.Message.length > 0) {
                                            $().lawsDialog({
                                                messages: [resp.Message],
                                                showIcon: false,
                                                onClose: function () {
                                                    if (resp.Completed && resp.ReturnUrl !== void 0 && resp.ReturnUrl.length > 0) {
                                                        window.location.href = resp.ReturnUrl;
                                                    }
                                                }
                                            });
                                        }
                                    }
                                });
                            }
                        }
                    ]
                });
            });

        $('.delete-site').off('click').on('click', function (e) {
            var siteId = $(this).data('id');
            e.preventDefault();
            $().lawsDialog({
                messages: ['Xác nhận xóa site?']
                , buttons: [
                    {
                        text: 'Hủy',
                        class: 'btn-thongbao2',
                        click: function () {
                            $(this).dialog('close');
                        }
                    },
                    {
                        text: 'Đồng ý',
                        class: 'btn-thongbao1',
                        click: function () {
                            $(this).dialog('close');
                            $.lawsAjax({
                                url: cms.virtualPath('/Ajax/SiteDeleteById'),
                                data: { siteId: siteId },
                                success: function (resp) {
                                    if (resp.Completed) {
                                        if (resp.Message !== null && resp.Message.length > 0) {
                                            $().lawsDialog({
                                                dialogClass: 'lawsVnDialogTitle',
                                                messages: [resp.Message],
                                                onClose: function () {
                                                    if (resp.ReturnUrl !== null && resp.ReturnUrl.length > 0)
                                                        setTimeout(function () {
                                                            window.location.href = resp.ReturnUrl;
                                                            location.reload();
                                                        },
                                                            100);
                                                    else window.location.href = cmsConfigs.rootPath;
                                                }
                                            });
                                        }
                                    }
                                }
                            });
                        }
                    }
                ]
            });
        });

        $('.delete-sites').off('click').on('click',
            function (event) {
                event.preventDefault();
                var sitesId = $('.chk-action input[type=checkbox]:checked').map(function (_, el) {
                    return $(el).val();
                }).get();
                if (!sitesId.length) {
                    $().lawsDialog({
                        dialogClass: 'lawsVnDialogTitle',
                        messages: ['Bạn chưa chọn site cần xóa.']
                    });
                    return;
                }
                $().lawsDialog({
                    dialogClass: 'lawsVnDialogTitle',
                    messages: ['Xác nhận xóa site đã chọn ?'],
                    buttons: [
                        {
                            text: 'Đóng',
                            class: 'btn-thongbao2',
                            click: function () {
                                $(this).dialog('close');
                            }
                        },
                        {
                            text: 'ĐỒNG Ý',
                            class: 'btn-thongbao1',
                            click: function () {
                                $(this).dialog('close');
                                $.lawsAjax({
                                    url: cms.virtualPath('/Ajax/SiteDelete'),
                                    type: 'Post',
                                    traditional: true,
                                    data: { sitesId: sitesId },
                                    success: function (resp) {
                                        if (resp.Message !== void 0 && resp.Message.length > 0) {
                                            $().lawsDialog({
                                                messages: [resp.Message],
                                                showIcon: false,
                                                onClose: function () {
                                                    if (resp.Completed && resp.ReturnUrl !== void 0 && resp.ReturnUrl.length > 0) {
                                                        window.location.href = resp.ReturnUrl;
                                                    }
                                                }
                                            });
                                        }
                                    }
                                });
                            }
                        }
                    ]
                });
            });

        $('.media-select').off('click').on('click',
            function (event) { 
                event.preventDefault();
                var mediaId = $(this).data('id');
                $.lawsAjax({
                    url: cms.virtualPath('/Ajax/MediaSelect'),
                    type: 'Post',
                    traditional: true,
                    data: { mediaId: mediaId },
                    success: function (resp) {
                        if (resp.Completed) {
                            if (resp.Message !== void 0 && resp.Message.length > 0) {
                                var imageSelect = window.opener.document.getElementById('SelectImage');
                                var filePath = window.opener.document.getElementById('ImagePath');
                                filePath.value = resp.Message;
                                imageSelect.src = cms.virtualPath('/' + resp.Message); 
                                window.close();
                            }
                        }
                    }
                });
            });

        $(document).on('change',
            '.danh-sach-loai-san-pham',
            function (event) {
                event.preventDefault();
                var dataTypeId = $(this).val();
                var ddlArticleTypeId = $("#ArticleTypeId");
                $.lawsAjax({
                    url: cms.virtualPath('/Ajax/ArticleTypesByDataTypeId'),
                    type:'Post',
                    data: {
                        dataTypeId: dataTypeId
                    },
                    success: function (resp) {
                        if (resp.Completed) {
                            ddlArticleTypeId.html($('<option></option>').val(0).html('...'));
                            $.each(resp.Data,
                                function(i, option) {
                                    ddlArticleTypeId.append($('<option></option>').val(option.ArticleTypeId)
                                        .html(option.ArticleTypeDesc));
                                });
                        } else {
                            if (resp.Message != null && resp.Message.length > 0) {
                                $().lawsDialog({
                                    messages: [resp.Message]
                                });
                            }
                        }
                    }
                });
            });
    },
    ajaxEvents: {
        OnBegin: function () {
            $('#loading').fadeIn('normal');
        },
        OnComplete: function (element) {
            $('#loading').fadeOut('normal');
            if (typeof element != undefined &&
                element != null &&
                element.length) {
                element = element + 'Tab';
                if (typeof element != undefined &&
                    element != null &&
                    element.length) {
                    $('html, body').animate({
                        scrollTop: $('#' + element).offset().top
                    },
                        1000);
                }
            }
        },
        OnSuccess: function (data, status, xhr) {
            if (data.Message !== void 0 && data.Message.length > 0) {
                if (data.Message == 'ModelStateInValid') {
                    $().lawsDialog({
                        messages: ['Quý khách vui lòng kiểm tra lại các thông tin có cảnh báo màu đỏ.'],
                        showIcon: false
                    });
                } else
                    $().lawsDialog({
                        messages: [data.Message],
                        showIcon: false
                    });
            }
        },
        OnSuccessRedirect: function (data, status, xhr) {
            if (data.Message !== null && data.Message.length > 0) {
                $().lawsDialog({
                    messages: [data.Message],
                    onClose: function () {
                        window.parent.jQuery('#divEdit').dialog('close');
                        if (data.Completed) {
                            if (data.ReturnUrl !== null && data.ReturnUrl.length > 0)
                                setTimeout(function () {
                                    window.location.href = data.ReturnUrl;
                                    location.reload();
                                }, 100);
                            else window.location.href = cmsConfigs.rootPath;
                        }
                    }
                });
            }
        },
        LaweQuestionOnSuccess: function (data, status, xhr) {
            if (data.Completed) {
                $().lawsDialog({
                    messages: [data.Message],
                    showIcon: false,
                    onClose: function () {
                        window.location.href = data.ReturnUrl;
                        location.reload();
                    }
                });
            }
            else if (data.Message !== void 0 && data.Message.length > 0) {
                $().lawsDialog({
                    messages: [data.Message],
                    showIcon: false
                });
            }
        },
        LoginOnSuccessful: function (data, status, xhr) {
            if (data.Completed) {
                $().lawsDialog({
                    messages: [data.Message],
                    showIcon: false,
                    onOpen: function () {
                        $("#lawsVnLogin").dialog("close");
                    },
                    onClose: function () {
                        if (data.ReturnUrl !== void 0 && data.ReturnUrl.length > 0)
                            setTimeout(function () {
                                window.location.href = data.ReturnUrl;
                                location.reload();
                            }, 100);
                        else window.location.href = lawsVnConfig.rootPath;
                    }
                });
            }
            else if (data.Message !== void 0 && data.Message.length > 0) {
                $().lawsDialog({
                    messages: [data.Message],
                    showIcon: false
                });
            }
        },
        LoginOnSuccess: function (data, status, xhr) {
            if (data.Completed) {
                $().lawsDialog({
                    messages: [data.Message],
                    showIcon: false,
                    onOpen: function () {
                        $("#lawsVnLogin").dialog("close");
                    },
                    onClose: function () {
                        if (data.ReturnUrl !== void 0 && data.ReturnUrl.length > 0)
                            //setTimeout(function () {
                            window.location.href = data.ReturnUrl;
                        //location.reload();
                        //}, 100);
                        else window.location.href = lawsVnConfig.rootPath;
                    }
                });
            } else {
                if (data.Message === 'ModelStateInValid') {
                    $().lawsDialog({
                        messages: ['Quý khách vui lòng kiểm tra lại các thông tin có cảnh báo màu đỏ.'],
                        showIcon: false
                    });
                } else
                    $().lawsDialog({
                        messages: [data.Message],
                        showIcon: false
                    });
            }
        },
        ServiceRegistrationOnSuccess: function (data, status, xh) {
            if (data.Completed) {
                $('#OrderCode').html('Mã đơn hàng dịch vụ của Quý khách:<strong style="color: #a67942;"> ' + data.Message + ' </strong>');
            } else {
                $().lawsDialog({
                    messages: [data.Message],
                    showIcon: false
                });
            }
        },
        PromotionCodeCheckOnSuccess: function (data, status, xhr) {
            if (data.Completed) {
                if (data.Message !== void 0 && data.Message.length > 0) {
                    $('#promotionCodeCheckResult').html(data.Message);
                }
                if (data.Data !== void 0 && data.Data.length > 0) {
                    $('#PromotionCodeBankAccount').lawsExists(function () {
                        $(this).val(data.Data);
                    });
                    $('#PromotionCodeScratchCard').lawsExists(function () {
                        $(this).val(data.Data);
                    });
                }
            } else {
                if (data.Message !== void 0 && data.Message.length > 0) {
                    $().lawsDialog({
                        messages: [data.Message]
                    });
                }
                $('#promotionCodeCheckResult').empty();
                $('#PromotionCodeBankAccount').lawsExists(function () {
                    $(this).val('');
                });
                $('#PromotionCodeScratchCard').lawsExists(function () {
                    $(this).val('');
                });
            }
        },
        TaxInvoiceOnComplete: function (data, status, xhr) {
            if (data.Completed) {
                if (data.Data !== void 0 && data.Data !== null) {
                    $('input[name="CompanyName"]').val(data.Data.CompanyName);
                    $('input[name="CompanyAddress"]').val(data.Data.CompanyAddress);
                    $('input[name="CompanyTaxCode"]').val(data.Data.CompanyTaxCode);
                    $('input[name="InternalContent"]').val(data.Data.InternalContent);
                }
                $('#TaxInvoiceFormLoad').dialog('close');
            } else if (data.Message !== void 0 && data.Message.length > 0) {
                $().lawsDialog({
                    messages: [data.Message],
                    showIcon: false
                });
            }

        },
        OnSuccessValid: function (element) {
            $('#loading').fadeOut('normal');
            $('#' + element).lawsExists(function () {
                var form = $('#' + element);
                form.removeData('validator');
                form.removeData('unobtrusiveValidation');
                $.validator.unobtrusive.parse(form);
            });
        }
    },
    virtualPath: function (patch) {
        var host = window.location.protocol + '//' + window.location.host;
        return host + patch;
    },
    MediaSelect: function (mediaPath) {
        var imgSelect = window.opener.document.getElementById('ImageSelect');
            var filePath = window.opener.document.getElementById('FilePath');
            filePath.value = mediaPath;
            imgSelect.src = mediaPath;
            window.close();
    }
}

$.fn.lawsExists = function (callback) {
    var args = [].slice.call(arguments, 1);
    if (this.length) {
        callback.call(this, args);
    }
    return this;
}

$.extend({
    lawsVnAjax: function (url, type, dataGetter, onsuccess) {
        var execOnSuccess = $.isFunction(onsuccess) ? onsuccess : $.noop;
        var getData = $.isFunction(dataGetter) ? dataGetter : function () { return dataGetter; };
        $.ajax({
            url: url,
            type: type,
            traditional: true,
            data: getData(),
            beforeSend: function () {
                $('#loadmore').prop('disabled', true).css('cursor', 'wait').text('Đang tải dữ liệu...');
            },
            error: function (jqXhr, errorMessage) {
                if (jqXhr.status === 0) {
                    lawsVn.dialog({
                        messages: ['Không có kết nối mạng. Vui lòng kiểm tra lại.']
                        , showIcon: false
                    });
                } else if (jqXhr.status == 404) {
                    lawsVn.dialog({
                        messages: ['Không tìm thấy trang yêu cầu. [404]']
                        , showIcon: false
                    });
                } else if (jqXhr.status == 500) {
                    lawsVn.dialog({
                        messages: ['Lỗi máy chủ nội bộ. [500].']
                        , showIcon: false
                    });
                } else if (errorMessage === 'parsererror') {
                    lawsVn.dialog({
                        messages: ['Yêu cầu phân tích cú pháp JSON lỗi.']
                        , showIcon: false
                    });
                } else if (errorMessage === 'timeout') {
                    lawsVn.dialog({
                        messages: ['Hết thời gian yêu cầu.']
                        , showIcon: false
                    });
                } else if (errorMessage === 'abort') {
                    lawsVn.dialog({
                        messages: ['Yêu cầu xử lý bị hủy.']
                        , showIcon: false
                    });
                } else if (jqXhr.status != 403) {
                    lawsVn.dialog({
                        messages: ['Lỗi :.n' + jqXhr.responseText]
                        , showIcon: false
                    });
                }
                $('#loadmore').prop('disabled', true).css('cursor', 'default').text('Xem thêm');
            },
            success: function (data, status, xhr) {
                window.setTimeout(function () {
                    execOnSuccess(data);
                }, 10);
                $('#loadmore').prop('disabled', true).css('cursor', 'default').text('Xem thêm');
            }
        });
    }
});

(function ($) {
    $.fn.lawsDialog = function (options) {
        var defaultOptions = {
            title: 'Thông báo',
            width: 'auto',
            height: 'auto',
            minWidth: 'auto',
            minHeight: 'auto',
            resizable: false,
            autoOpen: true,
            modal: true,
            show: 'fade',
            hide: 'blind',
            closeText: "Đóng",
            position: { my: "center", at: "top+150", of: window.top },
            dialogClass: 'lawsVnDialog',
            buttons: null,
            onCreate: {},
            onOpen: {},
            onClose: {},
            hideClose: true,
            showIcon: false, //hiện icon chuông hay ko
            messages: []
        };
        if (typeof options == 'object') {
            options = $.extend(defaultOptions, options);
        } else {
            options = defaultOptions;
        }
        var self = this;
        var execOnClose = $.isFunction(options.onClose) ? options.onClose : $.noop;
        var execOnOpen = $.isFunction(options.onOpen) ? options.onOpen : $.noop;
        var execOnCreate = $.isFunction(options.onCreate) ? options.onCreate : $.noop;
        options.messages = $.isArray(options.messages) ? options.messages : [];

        var html = '<div class="content-thongbao">' +
            '<div class="rows-thongbao" style=" font-size: 15px;font-weight: bold; line-height: 24px;">';
        if (options.showIcon) {
            html += '<img alt="img-tb" class="img-tb" src="' + lawsVnConfig.rootPath + 'assets/images/icon-tb.png">';
        }
        html += options.messages[0] +
            '</div>';
        if (options.messages.length > 1) {
            html +=
                '<div class="rows-thongbao center" style="font-size: 13px; font-style: italic; line-height: 24px;">' +
                '<span>' + options.messages[1] + '</span> <br>';
        }
        if (options.messages.length > 2) {
            html += '<span style="color: #d81c22">' + options.messages[2] + '</span>';
        }
        html += '</div>';
        if (!self.length) {
            self = $(html);
        }
        self.dialog({
            title: options.title,
            width: options.width,
            height: options.height,
            minWidth: options.minWidth,
            minHeight: options.minHeight,
            resizable: options.resizable,
            autoOpen: options.autoOpen,
            modal: options.modal,
            closeText: options.closeText,
            position: options.position,
            dialogClass: options.dialogClass,
            show: options.show,
            hide: options.hide,
            buttons: options.buttons || [
                {
                    text: 'Đóng',
                    class: 'btn-thongbao1',
                    click: function () {
                        $(self).dialog('close');
                        window.setTimeout(function () {
                            execOnClose();
                        }, 10);
                    }
                }
            ],
            create: function () {
                window.setTimeout(function () {
                    execOnCreate();
                }, 10);
            },
            open: function (event, ui) {
                $(self).closest(".ui-dialog").find(":button").blur();
                $(this).parents('.ui-dialog-buttonpane button:eq(0)').focus();
                //ẩn nút đóng x
                if (options.hideClose) {
                    $(self).parent().children().children('.ui-dialog-titlebar-close').hide();
                }
                if (options.title === '') {
                    //$(this).siblings('.ui-dialog-titlebar').remove();
                    $(this).dialog("widget").find(".ui-dialog-titlebar").remove();
                }
                window.setTimeout(function () {
                    execOnOpen(event, ui);
                }, 10);
            },
            close: function (event, ui) {
                $(self).dialog('close');
                $(self).dialog('destroy').remove();
                window.setTimeout(function () {
                    execOnClose(event, ui);
                }, 10);
            }
        });
    }
})(jQuery);

$.extend({
    lawsAjax: function (options) {
        var defaults =
            {
                url: '',
                type: 'Get',
                data: {},
                dataType: 'json',
                async: false,
                cache: false,
                traditional: false,
                timeout: 5000,
                beforeSend: function () {
                    $('#loading').fadeIn('normal');
                },
                success: function (data, status, xhr) {
                    if (data.Message !== void 0 && data.Message.length > 0) {
                        $().lawsDialog({
                            messages: [data.Message],
                            showIcon: false,
                            onClose: function () {
                                if (data.ReturnUrl !== void 0 && data.ReturnUrl.length > 0) {
                                    window.location.href = data.ReturnUrl;
                                }
                            }
                        });
                    }
                },
                error: function (jqXhr, errorMessage) {
                    $('#loading').fadeOut('normal');
                    if (jqXhr.status === 0) {
                        $().lawsDialog({
                            messages: ['Không có kết nối mạng. Vui lòng kiểm tra lại.']
                            , showIcon: false
                        });
                    } else if (jqXhr.status === 404) {
                        $().lawsDialog({
                            messages: ['Không tìm thấy trang yêu cầu. [404]']
                            , showIcon: false
                        });
                    } else if (jqXhr.status === 500) {
                        $().lawsDialog({
                            messages: ['Lỗi máy chủ nội bộ. [500].']
                            , showIcon: false
                        });
                    } else if (errorMessage === 'parsererror') {
                        $().lawsDialog({
                            messages: ['Yêu cầu phân tích cú pháp JSON lỗi.']
                            , showIcon: false
                        });
                    } else if (errorMessage === 'timeout') {
                        $().lawsDialog({
                            messages: ['Hết thời gian yêu cầu.']
                            , showIcon: false
                        });
                    } else if (errorMessage === 'abort') {
                        $().lawsDialog({
                            messages: ['Yêu cầu xử lý bị hủy.']
                            , showIcon: false
                        });
                    } else if (jqXhr.status !== 403) {
                        $().lawsDialog({
                            messages: ['Lỗi :.n' + jqXhr.responseText]
                            , showIcon: false
                        });
                    }
                },
                always: function () {
                    $('#loading').fadeOut('normal');
                }
            }
        options = $.extend(defaults, options);
        if (options.url.length) {
            $.ajax({
                url: options.url,
                type: options.type,
                data: options.data,
                dataType: options.dataType,
                async: options.async,
                cache: options.cache,
                traditional: options.traditional,
                timeout: options.timeout,
                beforeSend: function () {
                    if ($.isFunction(options.beforeSend)) {
                        window.setTimeout(function () {
                            options.beforeSend();
                        }, 10);
                    }
                },
                success: function (data, status, xhr) {
                    if ($.isFunction(options.success)) {
                        window.setTimeout(function () {
                            options.success(data, status, xhr);
                        }, 10);
                    }
                },
                error: function (jqXhr, errorMessage) {
                    window.setTimeout(function () {
                        options.error(jqXhr, errorMessage);
                    }, 10);
                }
            }).always(function () {
                window.setTimeout(function () {
                    options.always();
                }, 10);
            });
        }
    }
});

$(document).ready(function () {
    var url_cur = location.href;
    if (url_cur.indexOf("#") > -1 && url_cur.indexOf('huong-dan.html') > -1) {
        idx = url_cur.indexOf("#");
        var str_id_guid = idx != -1 ? url_cur.substring(idx + 1) : "";
        if (str_id_guid.length) {
            var btn_guid = $('#' + str_id_guid);
            if (btn_guid.length) {
                var li_parent = btn_guid.parent();
                if (!li_parent.hasClass('active')) {
                    btn_guid.trigger('click');
                }
            }
        }
    }
});