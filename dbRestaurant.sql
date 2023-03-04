create database dbRestaurant
go

use dbRestaurant
go

create table metrics
(
	metricID int primary key identity(1,1),
	description varchar(255),
	campaignID int,
	display_order int
)

insert into metrics(description, campaignID,display_order)
	values('pascal', 22, 1)

	insert into metrics(description, campaignID,display_order)
	values('oats', 22, 2)

	insert into metrics(description, campaignID,display_order)
	values('jungles', 22, 3)

		insert into metrics(description, campaignID,display_order)
	values('add', 22, 4)

		insert into metrics(description, campaignID,display_order)
	values('kim', 22, 5)