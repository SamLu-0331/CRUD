{{- define "app.hosts.httpapi" -}}
{{- print "https://" (.Values.global.hosts.httpapi | replace "[RELEASE_NAME]" .Release.Name) -}}
{{- end -}}
{{- define "app.hosts.blazor" -}}
{{- print "https://" (.Values.global.hosts.blazor | replace "[RELEASE_NAME]" .Release.Name) -}}
{{- end -}}
