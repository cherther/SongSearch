$(document).ready(function () {

	//------------------------------------------------------------------------------------
	// Catalog Upload Wizard
	//------------------------------------------------------------------------------------
	if (uploadWidget.length > 0) {
		//------------------------------------------------------------------------------------
		// Upload Widget - Setup
		//------------------------------------------------------------------------------------
		uploadWidget.pluploadQueue(uploadOptions);

		var uploader = uploadWidget.pluploadQueue();

		//------------------------------------------------------------------------------------
		// Upload Event Binding
		//------------------------------------------------------------------------------------
		uploader.bind('Init', function (up, params) {
			debug('Runtime: ' + params.runtime);
		});
		uploader.bind('FilesAdded', function (up, files) {

			debug('FilesAdded event fired');
			//checkTotalUploadSize(up);
		});

		uploader.bind('QueueChanged', function (up, files) {
			debug('QueueChanged event fired');
			if (checkTotalUploadSize(up)) {
				var fileList = $('#fileList');
				fileList.html('');
				$.each(up.files, function (i, file) {
					fileList.append('<input type="hidden" name="state.TempFiles[' + i + ']" value="' + file.name + '" />');
				});
			} else {
				return false;
			}
		});
		//		uploader.bind('UploadProgress', function () {
		//			notifyFilesDone(uploader);
		//		});
		//uploader.bind('UploadProgress', function (up, file) { notifyFilesDone(up); });

		uploader.bind('Error', function (up, error) {

			handleError(up, error);

			turnStepActionButtonOn(true);
		});


		//------------------------------------------------------------------------------------
		// Upload Form
		//------------------------------------------------------------------------------------


		// Client side form validation

		catalogUploadForm.submit(function (evt) {
			var formUp = $(wizardUploadWidgetId).pluploadQueue();

			//			uploader.unbind('UploadProgress');//, function (up, file) { notifyFilesDone(up); });
			//			formUp.unbind('UploadProgress');//, function (up, file) { notifyFilesDone(up); });

			// Validate number of uploaded files
			if (formUp.total.uploaded == 0) {
				// Files in queue upload them first
				var min = $('#minimumFiles').val();
				min = min != null ? min : 0;

				if (formUp.files.length >= min) {

					if (formUp.files.length > 0) {

						if (!checkTotalUploadSize(formUp)) {
							evt.preventDefault();
							return;
						} // When all files are uploaded submit form


						//						formUp.bind('UploadProgress', function (up, file) {
						//							debug('UploadProgress event fired');
						//							
						//						});
						formUp.bind('FileUploaded', function (up, file, response) {
							//debug('FileUploaded event fired');
							submitUploadFormWhenFilesDone(up);
						});
						formUp.bind('Error', function (up, error) {

							handleError(up, error);
							$.prettyLoader.hide();
							turnStepActionButtonOn(true);
						});

						evt.preventDefault();
						$.prettyLoader.show();
						formUp.start();
						turnStepActionButtonOn(false);
					}
				} else {
					evt.preventDefault();
					feedback('info', 'Please select at least ' + min + pluralize(' file', min) + ' to upload.');
				}
			}
		});
	}

	//	$('.cw-media-upload').live('click', function (evt) {
	//		evt.preventDefault();
	//		var link = $(this);
	//		var selector = link.siblings('.cw-media-upload-selector');
	//		var container = link.parent();
	//		setupMediaUploader(container[0].id, selector[0].id);
	//		//link.hide();
	//		selector.show();
	//		//selector.trigger('click');
	//	});
});

//------------------------------------------------------------------------------------
// functions
//------------------------------------------------------------------------------------
var wizardUploadWidgetId = '#wizardUploader';
var uploadLinkClass = '.cw-media-upload';
var audioFileFilter = { title: "Audio files", extensions: "mp3" };
var uploadWidget = $(wizardUploadWidgetId);
//var FILE_EXTENSION_ERROR = -700;
var catalogUploadForm = $('#catalogUploadForm');
var catalogFormIsSubmitted = false;
var uploadOptions =
	{
		// General settings
		runtimes: 'gears,html5,flash,silverlight', //gears,flash,silverlight,
		url: '/CatalogUpload/UserMediaUpload',
		max_file_size: '20mb',
		chunk_size: '1mb',
		//unique_names: true,
		// Specify what files to browse for
		filters: [audioFileFilter], //[{ title: "Audio files", extensions: "mp3"}],
		// Flash settings
		flash_swf_url: '/public/flash/plupload.flash.swf',
		// Silverlight settings
		silverlight_xap_url: '/public/flash/plupload.silverlight.xap'
	};

function turnStepActionButtonOn(on) {
	if (on) {
		$('#stepAction').removeAttr('disabled');
	}
	else {
		$('#stepAction').attr('disabled', 'true');
	}
}
function submitUploadFormWhenFilesDone(up) {
	var done = 0;
	$.each(up.files, function (i, file) {
		done += up.files[i].status == plupload.DONE; // 5; // 
	});
	
	if (done == up.files.length) {//  && up.total.percent == 100) {
		//if (!catalogFormIsSubmitted) {
		//	catalogFormIsSubmitted = true;
		debug('catalogUploadForm submit at done = ' + done);
		catalogUploadForm.submit();
		//}

	}
}
//function notifyFilesDone(up) {
//	var done = 0;
//	$.each(up.files, function (i, file) {
//		done += up.files[i].percent == 100; //up.files[i].status == plupload.DONE;//.percent == 100;
//	});
//	if (done == up.files.length) {
//		feedback('info', done + pluralize(' file', done) + ' uploaded.');// or upload additional files');

//	}
//}
function checkTotalUploadSize(up) {

	var maxNumber = parseInt($('#maxFiles').val());
	var maxSize = parseInt($('#maxBytes').val());
	var totalSize = 0;
	var files = up.files;
	$.each(files, function (i, file) {
		totalSize += file.size;
	});
	if (files.length > maxNumber) {
		var msg = 'You may only upload a total number of ' + maxNumber + ' files (you added ' + files.length + ' files)';
		feedback('warning', msg);
		return false;
	} else if (totalSize > maxSize) {
		var msg = 'Total file size is more than ' + toFileSizeDescription(maxSize) + ' (' + toFileSizeDescription(totalSize) + '). If you have a slow upload link, this may take a long time and may time out.';
		feedback('info', msg);
		return true;
	}
	return true;
}
function handleError(up, error) {

	if (error != null) {
		switch (error.code) {
			case plupload.FILE_SIZE_ERROR:
				feedback('info', 'Please upload audio files smaller than ' + toFileSizeDescription(up.settings.max_file_size) + ' only.');
				break;
			case plupload.FILE_EXTENSION_ERROR:
				feedback('info', 'Please upload audio files in MP3 format only.');
				break;
			default:
				feedback('error', 'There was an trying to upload your file (' + error.message + ')');
		}
	}
}

var _mediaUploaders = new Array();
function resetUploaders() {
	_mediaUploaders = new Array();
}
function setupMediaUploader(container, browseButton, fileList, mediaVersion, index) {

	//uploadOptions['browse_button'] = 'fullVersionUpload';
	//uploadOptions['container'] = 'fullUploadContainer';

	var mediaUploader = new plupload.Uploader(
	{
		// General settings
		runtimes: 'gears,html5,flash,silverlight', //gears,flash,silverlight,
		url: '/CatalogUpload/UserMediaUpload?mediaVersion=' + mediaVersion,
		max_file_size: '20mb',
		chunk_size: '1mb',
		container: container, //'fullUploadContainer',
		browse_button: browseButton, //'fullVersionUpload',
		//unique_names: true,
		// Specify what files to browse for
		filters: [{ title: "Audio files", extensions: "mp3" }], //[{ title: "Audio files", extensions: "mp3"}],
		// Flash settings
		flash_swf_url: '/public/flash/plupload.flash.swf',
		// Silverlight settings
		silverlight_xap_url: '/public/flash/plupload.silverlight.xap'
	}
	);

	var fileList = $('#' + fileList);
	mediaUploader.bind('Init', function (up, params) {
		//$('#' + fileList).text("Current runtime: " + params.runtime);
		//$('#fullVersionUpload').text(up.settings['browse_button']);
	});
//	mediaUploader.bind('FilesAdded', function (up, files) {
//	});

	mediaUploader.bind('FilesAdded', function (up, files) {
		//		if (files.length > 0) {
		//			if (up.files.length > 1) {
		//				for(var i=0;i<up.files.length-1;i++){
		//					var file = up.files[i];
		//					up.removeFile(file);
		//				}
		//			}
		//			up.start();
		//		}
		//debug('FilesAdded event fired');

		//		if (up.files.length > 0) {
		//			up.files = files;
		//			//debug('Added ' + file);
		//		}
		
		$.each(files, function (i, file) {
			debug('FilesAdded: ' + file.name);
		});
		//debug('mediaUploader.start() in FilesAdded event');
	});

	mediaUploader.bind('QueueChanged', function (up) {
		debug('QueueChanged ' + up.files.length);
		if (up.files.length > 1) {
			for (var i = 0; i < up.files.length - 1; i++) {
				var file = up.files[i];
				up.removeFile(file);
				debug('Removed ' + file.name);
			}
		}
		fileList.html('');
		//var index = _mediaUploaders.indexOf(up);
		if (up.files.length > 0) {
			var file = up.files[0];
			var filePath = file.name;// input.value;
			fileList.html('<label>New file: </label><br/><label><strong>' + filePath + '</strong></label>');
			$('#uploadFiles_' + index + '_FileName').val(file.name);

			//			fileList.append('<input type="hidden" name="uploadFiles[' + index + '].FileMediaVersion" value="' + mediaVersion + '" />');
			//			fileList.append('<input type="hidden" name="uploadFiles[' + index + '].FileName" value="' + file.name + '" />');

			//notifyFilesDone(up);
		}
		up.start();
	});
//	mediaUploader.bind('UploadProgress', function (up) {
//		notifyFilesDone(up);
//	});
	mediaUploader.bind('FileUploaded', function (up, file, response) {
		debug('FileUploaded event fired');

	});
	mediaUploader.bind('StateChanged', function (up, file, response) {
		debug('StateChanged: ' + up.total.queued + ' queued, ' + up.total.uploaded + ' uploaded');

	});
	mediaUploader.bind('Error', function (up, error) {
		fileList.html('');
		handleError(up, error);
	});
	mediaUploader.bind('Init', function (up, params) {
		debug('Runtime: ' + params.runtime);
	});

	mediaUploader.init();
	
	//var i = _mediaUploaders.length > 0 ? _mediaUploaders.length + 1 : 0;
	_mediaUploaders[index] = mediaUploader;

//	_mediaUploader.bind('QueueChanged', function (up, files) {
//		$.each(up.files, function (i, file) {
//			fileList.append('<input type="hidden" name="content.UploadFiles[' + i + ']" value="' + file.name + '" />');
//		});

//	});
}

