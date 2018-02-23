function loadValue(objectNode, embedNode, paramMap) {
}
function commitValue(objectNode, embedNode, paramMap) {
}
CKEDITOR.dialog.add('mediaDialog', function (editor) {
    var previewPreloader,
			previewAreaHtml = '<div>' + CKEDITOR.tools.htmlEncode(editor.lang.common.preview) + '<br>' +
			'<div id="cke_FlashPreviewLoader' + CKEDITOR.tools.getNextNumber() + '" style="display:none"><div class="loading">&nbsp;</div></div>' +
			'<div id="cke_FlashPreviewBox' + CKEDITOR.tools.getNextNumber() + '" class="FlashPreviewBox"></div></div>';
    return {
        title: editor.lang.media.title,
        minWidth: 420,
        minHeight: 310,
        onShow: function () {
            // Clear previously saved elements.
            
        },
        onOk: function () {
            // If there's no selected object or embed, create one. Otherwise, reuse the
            // selected object and embed nodes.
            
        },

        onHide: function () {
            if (this.preview)
                this.preview.setHtml('');
        },

        contents: [
				{
				    id: 'info',
				    label: editor.lang.common.generalTab,
				    accessKey: 'I',
				    elements: [
					{
					    type: 'vbox',
					    padding: 0,
					    children: [
						{
						    type: 'hbox',
						    widths: ['280px', '110px'],
						    align: 'right',
						    children: [
							{
							    id: 'src',
							    type: 'text',
							    label: editor.lang.common.url,
							    required: true,
							    validate: CKEDITOR.dialog.validate.notEmpty(editor.lang.media.validateSrc),
							    setup: loadValue,
							    commit: commitValue,
							    onLoad: function () {
							        var dialog = this.getDialog(),
									updatePreview = function (src) {
									    // Query the preloader to figure out the url impacted by based href.
									    previewPreloader.setAttribute('flashvars', src);
									    dialog.preview.setHtml('<embed height="100%" width="100%" src="http://cxo.vn/ckeditor/plugins/jwplayer/jwplayer/player.swf" flashvars="' + CKEDITOR.tools.htmlEncode(previewPreloader.getAttribute('flashvars'))
											+ '" type="application/x-shockwave-media"></embed>');
									};
							        // Preview element
							        dialog.preview = dialog.getContentElement('info', 'preview').getElement().getChild(3);

							        // Sync on inital value loaded.
							        this.on('change', function (evt) {

							            if (evt.data && evt.data.value)
							                updatePreview(evt.data.value);
							        });
							        // Sync when input value changed.
							        this.getInputElement().on('change', function (evt) {

							            updatePreview(this.getValue());
							        }, this);
							    }
							},
							{
							    type: 'button',
							    id: 'browse',
							    filebrowser: 'info:src',
							    hidden: true,
							    // v-align with the 'src' field.
							    // TODO: We need something better than a fixed size here.
							    style: 'display:inline-block;margin-top:10px;',
							    label: editor.lang.common.browseServer
							}
						]
						}
					]
					},
					{
					    type: 'hbox',
					    widths: ['25%', '25%', '25%', '25%', '25%'],
					    children: [
						{
						    type: 'text',
						    id: 'width',
						    requiredContent: 'embed[width]',
						    style: 'width:95px',
						    label: editor.lang.common.width,
						    validate: CKEDITOR.dialog.validate.htmlLength(editor.lang.common.invalidHtmlLength.replace('%1', editor.lang.common.width)),
						    setup: loadValue,
						    commit: commitValue
						},
						{
						    type: 'text',
						    id: 'height',
						    requiredContent: 'embed[height]',
						    style: 'width:95px',
						    label: editor.lang.common.height,
						    validate: CKEDITOR.dialog.validate.htmlLength(editor.lang.common.invalidHtmlLength.replace('%1', editor.lang.common.height)),
						    setup: loadValue,
						    commit: commitValue
						},
						{
						    type: 'text',
						    id: 'hSpace',
						    requiredContent: 'embed[hspace]',
						    style: 'width:95px',
						    label: editor.lang.media.hSpace,
						    validate: CKEDITOR.dialog.validate.integer(editor.lang.media.validateHSpace),
						    setup: loadValue,
						    commit: commitValue
						},
						{
						    type: 'text',
						    id: 'vSpace',
						    requiredContent: 'embed[vspace]',
						    style: 'width:95px',
						    label: editor.lang.media.vSpace,
						    validate: CKEDITOR.dialog.validate.integer(editor.lang.media.validateVSpace),
						    setup: loadValue,
						    commit: commitValue
						}
					]
					},

					{
					    type: 'vbox',
					    children: [
						{
						    type: 'html',
						    id: 'preview',
						    style: 'width:95%;',
						    html: previewAreaHtml
						}
					]
					}
				]
				},
				{
				    id: 'Upload',
				    hidden: true,
				    filebrowser: 'uploadButton',
				    label: editor.lang.common.upload,
				    elements: [
					{
					    type: 'file',
					    id: 'upload',
					    label: editor.lang.common.upload,
					    size: 38
					},
					{
					    type: 'fileButton',
					    id: 'uploadButton',
					    label: editor.lang.common.uploadSubmit,
					    filebrowser: 'info:src',
					    'for': ['Upload', 'upload']
					}
				]
				},
				{
				    id: 'properties',
				    label: editor.lang.media.propertiesTab,
				    elements: [
					{
					    type: 'hbox',
					    widths: ['50%', '50%'],
					    children: [
						{
						    id: 'scale',
						    type: 'select',
						    requiredContent: 'embed[scale]',
						    label: editor.lang.media.scale,
						    'default': '',
						    style: 'width : 100%;',
						    items: [
							[editor.lang.common.notSet, ''],
							[editor.lang.media.scaleAll, 'showall'],
							[editor.lang.media.scaleNoBorder, 'noborder'],
							[editor.lang.media.scaleFit, 'exactfit']
							],
						    setup: loadValue,
						    commit: commitValue
						},
						{
						    id: 'allowScriptAccess',
						    type: 'select',
						    requiredContent: 'embed[allowscriptaccess]',
						    label: editor.lang.media.access,
						    'default': '',
						    style: 'width : 100%;',
						    items: [
							[editor.lang.common.notSet, ''],
							[editor.lang.media.accessAlways, 'always'],
							[editor.lang.media.accessSameDomain, 'samedomain'],
							[editor.lang.media.accessNever, 'never']
							],
						    setup: loadValue,
						    commit: commitValue
						}
					]
					},
					{
					    type: 'hbox',
					    widths: ['50%', '50%'],
					    children: [
						{
						    id: 'wmode',
						    type: 'select',
						    requiredContent: 'embed[wmode]',
						    label: editor.lang.media.windowMode,
						    'default': '',
						    style: 'width : 100%;',
						    items: [
							[editor.lang.common.notSet, ''],
							[editor.lang.media.windowModeWindow, 'window'],
							[editor.lang.media.windowModeOpaque, 'opaque'],
							[editor.lang.media.windowModeTransparent, 'transparent']
							],
						    setup: loadValue,
						    commit: commitValue
						},
						{
						    id: 'quality',
						    type: 'select',
						    requiredContent: 'embed[quality]',
						    label: editor.lang.media.quality,
						    'default': 'high',
						    style: 'width : 100%;',
						    items: [
							[editor.lang.common.notSet, ''],
							[editor.lang.media.qualityBest, 'best'],
							[editor.lang.media.qualityHigh, 'high'],
							[editor.lang.media.qualityAutoHigh, 'autohigh'],
							[editor.lang.media.qualityMedium, 'medium'],
							[editor.lang.media.qualityAutoLow, 'autolow'],
							[editor.lang.media.qualityLow, 'low']
							],
						    setup: loadValue,
						    commit: commitValue
						}
					]
					},
					{
					    type: 'hbox',
					    widths: ['50%', '50%'],
					    children: [
						{
						    id: 'align',
						    type: 'select',
						    requiredContent: 'object[align]',
						    label: editor.lang.common.align,
						    'default': '',
						    style: 'width : 100%;',
						    items: [
							[editor.lang.common.notSet, ''],
							[editor.lang.common.alignLeft, 'left'],
							[editor.lang.media.alignAbsBottom, 'absBottom'],
							[editor.lang.media.alignAbsMiddle, 'absMiddle'],
							[editor.lang.media.alignBaseline, 'baseline'],
							[editor.lang.common.alignBottom, 'bottom'],
							[editor.lang.common.alignMiddle, 'middle'],
							[editor.lang.common.alignRight, 'right'],
							[editor.lang.media.alignTextTop, 'textTop'],
							[editor.lang.common.alignTop, 'top']
							],
						    setup: loadValue,
						    commit: function (objectNode, embedNode, paramMap, extraStyles, extraAttributes) {
						        var value = this.getValue();
						        commitValue.apply(this, arguments);
						        value && (extraAttributes.align = value);
						    }
						},
						{
						    type: 'html',
						    html: '<div></div>'
						}
					]
					},
					{
					    type: 'fieldset',
					    label: CKEDITOR.tools.htmlEncode(editor.lang.media.flashvars),
					    children: [
						{
						    type: 'vbox',
						    padding: 0,
						    children: [
							{
							    type: 'checkbox',
							    id: 'menu',
							    label: editor.lang.media.chkMenu,
							    'default': true,
							    setup: loadValue,
							    commit: commitValue
							},
							{
							    type: 'checkbox',
							    id: 'play',
							    label: editor.lang.media.chkPlay,
							    'default': true,
							    setup: loadValue,
							    commit: commitValue
							},
							{
							    type: 'checkbox',
							    id: 'loop',
							    label: editor.lang.media.chkLoop,
							    'default': true,
							    setup: loadValue,
							    commit: commitValue
							},
							{
							    type: 'checkbox',
							    id: 'allowFullScreen',
							    label: editor.lang.media.chkFull,
							    'default': true,
							    setup: loadValue,
							    commit: commitValue
							}
						]
						}
					]
					}
				]
				},
				{
				    id: 'advanced',
				    label: editor.lang.common.advancedTab,
				    elements: [
					{
					    type: 'hbox',
					    children: [
						{
						    type: 'text',
						    id: 'id',
						    requiredContent: 'object[id]',
						    label: editor.lang.common.id,
						    setup: loadValue,
						    commit: commitValue
						}
					]
					},
					{
					    type: 'hbox',
					    widths: ['45%', '55%'],
					    children: [
						{
						    type: 'text',
						    id: 'bgcolor',
						    requiredContent: 'embed[bgcolor]',
						    label: editor.lang.media.bgcolor,
						    setup: loadValue,
						    commit: commitValue
						},
						{
						    type: 'text',
						    id: 'class',
						    requiredContent: 'embed(cke-xyz)', // Random text like 'xyz' will check if all are allowed.
						    label: editor.lang.common.cssClass,
						    setup: loadValue,
						    commit: commitValue
						}
					]
					},
					{
					    type: 'text',
					    id: 'style',
					    requiredContent: 'embed{cke-xyz}', // Random text like 'xyz' will check if all are allowed.
					    validate: CKEDITOR.dialog.validate.inlineStyle(editor.lang.common.invalidInlineStyle),
					    label: editor.lang.common.cssStyle,
					    setup: loadValue,
					    commit: commitValue
					}
				]
				}
			]
    };
});