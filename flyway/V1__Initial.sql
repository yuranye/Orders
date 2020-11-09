create table ORDERS
(
	ORDER_UUID uuid not null,
	PRODUCT_UUID uuid not null,
	DELIVERY_UUID uuid not null,
	COUNT int not null
);

create unique index orders_order_uuid_uindex
	on ORDERS (ORDER_UUID);

alter table ORDERS
	add constraint orders_pk
		primary key (ORDER_UUID);

