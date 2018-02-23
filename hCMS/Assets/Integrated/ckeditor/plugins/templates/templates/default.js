/*
 Copyright (c) 2003-2014, CKSource - Frederico Knabben. All rights reserved.
 For licensing, see LICENSE.md or http://ckeditor.com/license
*/
CKEDITOR.addTemplates("default",
{
    imagesPath: CKEDITOR.getUrl(CKEDITOR.plugins.getPath("templates") + "templates/images/"),
    templates:
    [
        {
            title: "Nội dung meeting",
            image: "templateMeeting.gif",
            description: "Mẫu nội dung meeting",
            html: '<table border="0" cellpadding="5" cellspacing="2" style=" border-collapse:collapse;  width:100%; "><tbody><tr><td colspan="2" style="background-color: #ff0000; padding-left:10px; padding-right:10px; color: #fff"><p><b>Monthly Meeting &amp; C-Suite Alumni 2013</b></p><p><b>Next generation Offshoring the Globalization of Innovation opportunities</b></p><p ><b>Date:</b>&nbsp;25 | 09 | 2013</p><p><b>Venue:</b>&nbsp;Sheraton Saigon Hotel &amp; Towers, Ho Chi Minh City</p></td></tr><tr><td colspan="2"><p style="text-align:center"><img alt="meeting" src=" " style="border:  none; width:600px; height: 220px" /></p></td></tr><tr><td style="vertical-align:top; width: 200px"><p><img alt="meeting" src=" " style="border: none; width:190px; height: 420px" /></p></td><td  style="vertical-align:top;">Nội dung</td></tr><tr><td colspan="2"><p style="text-align:center"><img src=" " alt="meeting"  style="border:  none; width:600px; height: 220px" /></p></td></tr></tbody></table>'
        },
        {
            title: "Ảnh và tiêu đề",
            image: "template1.gif",
            description: "Một hình ảnh chính với một tiêu đề và văn bản bao quanh hình ảnh.",
            html: '<h3><img src=" " alt="" style="margin-right: 10px" height="100" width="100" align="left" />Nhập tiêu đề</h3><p>Nhập nội dung</p>'
        },
        {
            title: "2 cột",
            image: "template2.gif",
            description:"Mẫu gồm 2 cột mỗi cột có tiêu đề và nội dung",
            html: '<table cellspacing="0" cellpadding="0" style="width:100%" border="0"><tr><td style="width:50%"><h3>Tiêu đề 1</h3></td><td></td><td style="width:50%"><h3>Tiêu đề 2</h3></td></tr><tr><td>Nội dung 1</td><td></td><td>Nội dung 2</td></tr></table><p>Nội dung khác.</p>'
        },
        {
            title: "Nội dung và bảng",
            image: "template3.gif",
            description: "Tiêu đề và bảng.",
            html: '<div style="width: 80%"><h3>Tiêu đề</h3><table style="width:150px;float: right" cellspacing="0" cellpadding="0" border="1"><caption style="border:solid 1px black"><strong>Tiêu đề bảng</strong></caption><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr><tr><td>&nbsp;</td><td>&nbsp;</td><td>&nbsp;</td></tr></table><p>Nội dung</p></div>'
        }
    ]
});