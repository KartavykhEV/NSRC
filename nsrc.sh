#!/bin/sh

wget http://control.neuronsm.ru/nsrc/nsrc.tar nsrc.tar
mkdir /opt/nsrc
rm -Rf /opt/nsrc/*
tar -xvf nsrc.tar -C /opt/nsrc


echo "[Unit]
Description=Нейрон СМ - сервис удаленного управления

[Service]
WorkingDirectory=/opt/nsrc/
ExecStart=/usr/bin/dotnet /opt/nsrc/NSRservice.dll
Restart=always
RestartSec=10
SyslogIdentifier=nsrc
User=root
Group=root
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target" > /etc/systemd/system/nsrc.service

systemctl enable nsrc


echo 'if $programname == ''nsrc'' then /opt/nsrc/logfile.log
& stop' > /etc/rsyslog.d/40-nsrc.conf
echo "" > /opt/nsrc/logfile.log
chown syslog:adm /opt/nsrc/logfile.log
systemctl restart rsyslog